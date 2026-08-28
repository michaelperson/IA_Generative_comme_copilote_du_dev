"""
Version refactorée du traitement de commande legacy.
La signature publique de process_order (méthode de OrderProcessor) est
volontairement inchangée (mêmes paramètres, même valeur de retour) afin que
les tests de caractérisation écrits à l'étape 2 continuent de passer sans
être modifiés (étape 4). Le calcul de la remise, de la taxe et la décision de
validation manager sont désormais délégués à des classes/fonctions dédiées
(principe de responsabilité unique).
"""
from __future__ import annotations

import enum
from typing import Callable, Optional, Protocol


class CustomerType(enum.Enum):
    """Catégories de client utilisées pour le calcul de la remise.
    Remplace les chaînes magiques "VIP" / "REGULAR" du code legacy."""

    STANDARD = "STANDARD"
    REGULAR = "REGULAR"
    VIP = "VIP"

    @staticmethod
    def parse(customer_type: str) -> "CustomerType":
        """Traduit la valeur texte legacy vers CustomerType, afin de garder
        l'API publique de OrderProcessor inchangée pour les tests de
        caractérisation existants."""
        if customer_type == "VIP":
            return CustomerType.VIP
        if customer_type == "REGULAR":
            return CustomerType.REGULAR
        return CustomerType.STANDARD


class Country(enum.Enum):
    """Pays gérés par le calcul de taxe. Remplace les chaînes magiques
    "BE" / "FR" / "DE" du code legacy."""

    BELGIUM = "BE"
    FRANCE = "FR"
    GERMANY = "DE"
    OTHER = "OTHER"

    @staticmethod
    def parse(country: str) -> "Country":
        """Traduit le code pays texte legacy vers Country."""
        if country == "BE":
            return Country.BELGIUM
        if country == "FR":
            return Country.FRANCE
        if country == "DE":
            return Country.GERMANY
        return Country.OTHER


_VIP_DISCOUNT = 0.20
_REGULAR_DISCOUNT = 0.05
_FIRST_ORDER_BONUS = 0.10


def calculate_discount(customer_type: CustomerType, is_first_order: bool) -> float:
    """Responsabilité unique : calculer le taux de remise applicable à une commande."""
    if customer_type is CustomerType.VIP:
        discount = _VIP_DISCOUNT
    elif customer_type is CustomerType.REGULAR:
        discount = _REGULAR_DISCOUNT
    else:
        discount = 0.0

    if is_first_order:
        discount += _FIRST_ORDER_BONUS

    return discount


_DEFAULT_TAX_RATE = 0.15
_TAX_RATES_BY_COUNTRY = {
    Country.BELGIUM: 0.21,
    Country.FRANCE: 0.20,
    Country.GERMANY: 0.19,
}


def calculate_tax(country: Country) -> float:
    """Responsabilité unique : calculer le taux de taxe applicable selon le
    pays, à partir d'une table de configuration (élimine la chaîne
    if/elif legacy)."""
    return _TAX_RATES_BY_COUNTRY.get(country, _DEFAULT_TAX_RATE)


_MANAGER_VALIDATION_THRESHOLD = 1000


def requires_manager_validation(total: float) -> bool:
    """Indique si le montant total dépasse le seuil nécessitant une validation manager."""
    return total > _MANAGER_VALIDATION_THRESHOLD


def notify_manager_for_validation(total: float) -> None:
    """Point d'extension destiné à notifier le manager par email.

    NON IMPLÉMENTÉ À DESSEIN : aucune notification n'est réellement envoyée,
    exactement comme dans le code legacy d'origine (TODO jamais implémenté).
    Ce refactoring rend volontairement ce manque visible plutôt que de le
    faire disparaître silencieusement ou d'inventer un envoi d'email qui n'a
    jamais existé — voir le débriefing du README de cet exercice.
    """
    # TODO: implémenter l'envoi d'email au manager.


class Logger(Protocol):
    """Abstraction de journalisation injectée dans OrderProcessor pour
    retirer l'effet de bord print() du cœur métier."""

    def log(self, message: str) -> None:
        ...


class ConsoleLogger:
    """Implémentation par défaut de Logger qui écrit sur la console,
    équivalente au print() du code legacy mais désormais interchangeable."""

    def log(self, message: str) -> None:
        print(message)


class OrderProcessor:
    """Traite une commande legacy en déléguant remise, taxe et décision de
    validation manager à des fonctions/classes à responsabilité unique."""

    def __init__(self, logger: Optional[Logger] = None):
        self._logger: Logger = logger if logger is not None else ConsoleLogger()

    def process_order(self, customer_type: str, amount: float, country: str, is_first_order: bool) -> str:
        customer_type_enum = CustomerType.parse(customer_type)
        country_enum = Country.parse(country)

        discount = calculate_discount(customer_type_enum, is_first_order)
        tax = calculate_tax(country_enum)

        total = amount * (1 - discount) * (1 + tax)

        if requires_manager_validation(total):
            # TODO (toujours pas implémenté) : notifier réellement le manager.
            notify_manager_for_validation(total)
            result = f"Commande de {total} EUR - necessite validation manager"
        else:
            result = f"Commande de {total} EUR - validee automatiquement"

        self._logger.log(f"LOG: order processed for {customer_type} total={total}")

        return result
