"""Erreurs métier du catalogue de tarifs."""


class TarifIntrouvableError(Exception):
    """Levée lorsqu'aucun tarif ne correspond au SKU demandé."""

    def __init__(self, sku: str):
        super().__init__(f"Aucun tarif ne correspond au SKU « {sku} ».")
        self.sku = sku
