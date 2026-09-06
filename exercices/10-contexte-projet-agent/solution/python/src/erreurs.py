"""Erreurs métier du catalogue de tarifs."""


class TarifIntrouvableError(Exception):
    """Levée lorsqu'aucun tarif ne correspond au SKU demandé."""

    def __init__(self, sku: str):
        super().__init__(f"Aucun tarif ne correspond au SKU « {sku} ».")
        self.sku = sku


class PourcentageRemiseInvalideError(Exception):
    """Levée lorsque le pourcentage de remise sort de l'intervalle autorisé."""

    def __init__(self, pourcentage):
        super().__init__(
            f"Le pourcentage de remise doit être un entier de 0 à 100, reçu {pourcentage}."
        )
        self.pourcentage = pourcentage
