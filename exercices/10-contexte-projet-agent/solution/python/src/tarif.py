"""Tarif catalogue."""

from dataclasses import dataclass


@dataclass(frozen=True)
class Tarif:
    """Objet de valeur immuable. Le montant est exprimé en centimes entiers.

    Attributes:
        sku: Référence produit.
        libelle: Libellé commercial.
        prix_centimes: Prix unitaire, en centimes entiers.
    """

    sku: str
    libelle: str
    prix_centimes: int
