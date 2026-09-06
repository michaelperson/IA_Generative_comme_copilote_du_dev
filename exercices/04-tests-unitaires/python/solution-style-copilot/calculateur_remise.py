"""
Calcule le montant de la remise applicable à une commande, en fonction de
l'ancienneté du client et de son statut premium.

Fonction métier fournie pour l'exercice 4 (tests unitaires) : elle est déjà
complète et correcte. L'objectif de l'exercice est d'écrire, avec l'aide de
l'IA, la suite de tests unitaires qui la couvre (cf. test_calculateur_remise.py).
"""
from decimal import ROUND_HALF_UP, Decimal


def calculer_montant_remise(montant_commande: Decimal, annees_fidelite: int, est_client_premium: bool) -> Decimal:
    """
    Calcule le montant de la remise (en euros), arrondi à 2 décimales
    (arrondi "loin de zéro", équivalent de ROUND_HALF_UP pour des montants positifs).

    Lève ValueError si montant_commande est négatif.
    """
    if montant_commande < 0:
        raise ValueError("montant_commande doit être positif ou nul")

    if annees_fidelite >= 10:
        taux_base = Decimal("0.15")
    elif annees_fidelite >= 5:
        taux_base = Decimal("0.10")
    elif annees_fidelite >= 1:
        taux_base = Decimal("0.05")
    else:
        taux_base = Decimal("0")

    taux = taux_base + Decimal("0.05") if est_client_premium else taux_base
    taux = min(taux, Decimal("0.25"))  # plafond contractuel à 25%

    return (montant_commande * taux).quantize(Decimal("0.01"), rounding=ROUND_HALF_UP)
