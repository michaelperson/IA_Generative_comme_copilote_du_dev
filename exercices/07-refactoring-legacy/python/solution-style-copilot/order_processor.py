"""Version refactorée du traitement de commande legacy."""
from __future__ import annotations

import enum
from typing import Protocol

FIRST_ORDER_BONUS = 0.10
MANAGER_THRESHOLD = 1000


class CustomerType(enum.Enum):
    STANDARD = "STANDARD"
    REGULAR = "REGULAR"
    VIP = "VIP"

    @staticmethod
    def parse(value: str) -> "CustomerType":
        if value == "VIP":
            return CustomerType.VIP
        if value == "REGULAR":
            return CustomerType.REGULAR
        return CustomerType.STANDARD


class Country(enum.Enum):
    BELGIUM = "BE"
    FRANCE = "FR"
    GERMANY = "DE"
    OTHER = "OTHER"

    @staticmethod
    def parse(value: str) -> "Country":
        if value in ("BE", "FR", "DE"):
            return Country(value)
        return Country.OTHER


DISCOUNTS = {CustomerType.VIP: 0.20, CustomerType.REGULAR: 0.05, CustomerType.STANDARD: 0.0}
TAXES = {Country.BELGIUM: 0.21, Country.FRANCE: 0.20, Country.GERMANY: 0.19, Country.OTHER: 0.15}


class Logger(Protocol):
    def log(self, message: str) -> None: ...


class ConsoleLogger:
    def log(self, message: str) -> None:
        print(message)


def get_discount(customer_type: CustomerType, is_first_order: bool) -> float:
    discount = DISCOUNTS[customer_type]
    if is_first_order:
        discount += FIRST_ORDER_BONUS
    return discount


def get_tax(country: Country) -> float:
    return TAXES[country]


def needs_manager_validation(total: float) -> bool:
    return total > MANAGER_THRESHOLD


class OrderProcessor:
    def __init__(self, logger: Logger | None = None):
        self._logger = logger or ConsoleLogger()

    def process_order(self, customer_type: str, amount: float, country: str, is_first_order: bool) -> str:
        discount = get_discount(CustomerType.parse(customer_type), is_first_order)
        tax = get_tax(Country.parse(country))
        total = amount * (1 - discount) * (1 + tax)

        if needs_manager_validation(total):
            result = f"Commande de {total} EUR - necessite validation manager"
        else:
            result = f"Commande de {total} EUR - validee automatiquement"

        self._logger.log(f"LOG: order processed for {customer_type} total={total}")
        return result
