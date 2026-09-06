"""Consultation des tarifs du catalogue produit."""

from typing import Dict, Sequence

from erreurs import TarifIntrouvableError
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
