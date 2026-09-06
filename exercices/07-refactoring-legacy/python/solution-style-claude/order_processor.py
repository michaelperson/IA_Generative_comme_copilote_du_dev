"""Version refactorée du traitement de commande legacy.

Méthode appliquée, dans l'ordre imposé par l'exercice :

1. Responsabilité unique : le calcul de la remise, celui de la taxe et la
   décision de validation manager sont trois préoccupations distinctes, chacune
   dans sa propre classe. ``OrderProcessor`` ne fait plus que coordonner — c'est
   le signe que le principe est réellement appliqué et pas seulement que le code
   a été découpé.
2. DRY et chaînes magiques : les littéraux "VIP", "BE" et consorts, disséminés
   dans les branches du code d'origine, deviennent des énumérations, et les
   chaînes de ``if/elif`` deviennent des tables. Ajouter un pays est désormais
   une ligne de données, pas une branche de code.
3. Effet de bord : le ``print`` du cœur métier est remplacé par une interface de
   journalisation injectée.
4. Vérification : la signature publique de ``process_order`` est inchangée, les
   tests de caractérisation passent sans avoir été modifiés.

Point d'attention pour la relecture : l'ordre des opérations arithmétiques
reproduit exactement celui du code d'origine. En arithmétique flottante,
0.20 + 0.10 ne vaut pas exactement 0.30, et inverser l'ordre changerait le
résultat au dernier chiffre significatif.
"""
from __future__ import annotations

import enum
from typing import Optional, Protocol

_BONUS_PREMIERE_COMMANDE = 0.10
_SEUIL_VALIDATION_MANAGER = 1000


class CustomerType(enum.Enum):
    """Catégorie de client, en remplacement des chaînes magiques du legacy."""

    STANDARD = "STANDARD"
    REGULAR = "REGULAR"
    VIP = "VIP"

    @staticmethod
    def parse(valeur: str) -> "CustomerType":
        """Traduit la valeur texte de l'API publique.

        Retombe volontairement sur STANDARD pour toute valeur inconnue, au lieu
        de lever une exception : c'est le comportement du code d'origine, et un
        refactoring ne le change pas sans décision explicite.
        """
        if valeur == "VIP":
            return CustomerType.VIP
        if valeur == "REGULAR":
            return CustomerType.REGULAR
        return CustomerType.STANDARD


class Country(enum.Enum):
    """Pays gérés par le calcul de taxe."""

    BELGIUM = "BE"
    FRANCE = "FR"
    GERMANY = "DE"
    OTHER = "OTHER"

    @staticmethod
    def parse(valeur: str) -> "Country":
        if valeur in ("BE", "FR", "DE"):
            return Country(valeur)
        return Country.OTHER


class DiscountCalculator:
    """Responsabilité unique : le taux de remise. Ne connaît ni la taxe, ni le seuil."""

    _TAUX_DE_BASE = {
        CustomerType.VIP: 0.20,
        CustomerType.REGULAR: 0.05,
        CustomerType.STANDARD: 0.0,
    }

    def calculate(self, customer_type: CustomerType, is_first_order: bool) -> float:
        """Retourne le taux de remise sous forme de fraction (0.30 pour 30 %)."""
        discount = self._TAUX_DE_BASE[customer_type]
        if is_first_order:
            discount += _BONUS_PREMIERE_COMMANDE
        return discount


class TaxCalculator:
    """Responsabilité unique : le taux de TVA applicable au pays."""

    _TAUX_PAR_PAYS = {
        Country.BELGIUM: 0.21,
        Country.FRANCE: 0.20,
        Country.GERMANY: 0.19,
        Country.OTHER: 0.15,
    }

    def rate_for(self, country: Country) -> float:
        return self._TAUX_PAR_PAYS[country]


class ManagerValidationService:
    """Décide si une commande requiert une validation manager.

    Gap connu, hérité du code d'origine
    -----------------------------------
    Le code legacy portait le commentaire ``# TODO: envoyer un email au manager,
    jamais implémenté``. Ce comportement n'a **jamais existé**. Deux tentations
    se présentent pendant un refactoring assisté, toutes deux mauvaises :

    * faire disparaître le TODO avec le code qui l'entourait — l'information est
      alors perdue et personne ne saura plus que la notification manquait ;
    * implémenter l'envoi de sa propre initiative — on introduit un comportement
      nouveau que les tests de caractérisation ne détecteront pas, puisqu'ils ne
      le testaient pas.

    Troisième voie, retenue ici : rendre le manque explicite et bloquant, pour
    qu'il soit spécifié avant d'être implémenté.
    """

    def requires_validation(self, total: float) -> bool:
        return total > _SEUIL_VALIDATION_MANAGER

    def notify_manager(self, total: float) -> None:
        raise NotImplementedError(
            "Notification du manager non implémentée — comportement absent du code "
            "d'origine, à spécifier avant toute implémentation."
        )


class OrderLogger(Protocol):
    """Journalisation injectée, extraite du cœur métier.

    Le code d'origine appelait ``print`` directement : impossible de tester la
    méthode sans polluer la sortie, impossible de rediriger les traces vers
    l'infrastructure de logs réelle sans toucher à la logique métier.
    """

    def log(self, message: str) -> None: ...


class ConsoleOrderLogger:
    """Implémentation par défaut, équivalente au comportement d'origine."""

    def log(self, message: str) -> None:
        print(message)


class OrderProcessor:
    """Orchestration du traitement d'une commande.

    La signature publique est inchangée : les appelants existants n'ont rien à
    modifier, et les tests de caractérisation passent sans être touchés.
    """

    def __init__(
        self,
        discounts: Optional[DiscountCalculator] = None,
        taxes: Optional[TaxCalculator] = None,
        validation: Optional[ManagerValidationService] = None,
        logger: Optional[OrderLogger] = None,
    ):
        self._discounts = discounts or DiscountCalculator()
        self._taxes = taxes or TaxCalculator()
        self._validation = validation or ManagerValidationService()
        self._logger = logger or ConsoleOrderLogger()

    def process_order(
        self, customer_type: str, amount: float, country: str, is_first_order: bool
    ) -> str:
        discount = self._discounts.calculate(CustomerType.parse(customer_type), is_first_order)
        tax = self._taxes.rate_for(Country.parse(country))
        total = amount * (1 - discount) * (1 + tax)

        if self._validation.requires_validation(total):
            result = f"Commande de {total} EUR - necessite validation manager"
        else:
            result = f"Commande de {total} EUR - validee automatiquement"

        self._logger.log(f"LOG: order processed for {customer_type} total={total}")
        return result
