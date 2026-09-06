"""Consultation des tarifs du catalogue produit."""

from typing import Dict, Sequence

from erreurs import PourcentageRemiseInvalideError, TarifIntrouvableError
from tarif import Tarif


class CatalogueTarifs:
    """Lecture et calcul uniquement : aucune persistance, aucun appel réseau."""

    def __init__(self, tarifs: Sequence[Tarif]):
        self._par_sku: Dict[str, Tarif] = {t.sku: t for t in tarifs}

    def trouver_par_sku(self, sku: str) -> Tarif:
        """Retourne le tarif correspondant au SKU demandé.

        Args:
            sku: Référence produit recherchée.

        Returns:
            Le tarif correspondant.

        Raises:
            TarifIntrouvableError: si aucun tarif ne correspond.
        """
        tarif = self._par_sku.get(sku)
        if tarif is None:
            raise TarifIntrouvableError(sku)
        return tarif

    def appliquer_remise(self, sku: str, pourcentage: int) -> Tarif:
        """Retourne une nouvelle instance de tarif, remisée du pourcentage indiqué.

        Le calcul reste entièrement en centimes entiers : le produit
        prix x (100 - pourcentage) est exact, et seule la division finale par
        100 demande un arrondi, fait « loin de zéro » au centime le plus proche.

        Args:
            sku: Référence produit à remiser.
            pourcentage: Pourcentage de remise, entier de 0 à 100 inclus.

        Returns:
            Un nouveau Tarif ; le tarif d'origine est inchangé.

        Raises:
            TarifIntrouvableError: si aucun tarif ne correspond au SKU.
            PourcentageRemiseInvalideError: si le pourcentage n'est pas un
                entier compris entre 0 et 100.
        """
        if not isinstance(pourcentage, int) or isinstance(pourcentage, bool):
            raise PourcentageRemiseInvalideError(pourcentage)
        if pourcentage < 0 or pourcentage > 100:
            raise PourcentageRemiseInvalideError(pourcentage)

        tarif = self.trouver_par_sku(sku)

        produit = tarif.prix_centimes * (100 - pourcentage)
        quotient, reste = divmod(produit, 100)
        prix_remise_centimes = quotient + 1 if reste * 2 >= 100 else quotient

        return Tarif(tarif.sku, tarif.libelle, prix_remise_centimes)
