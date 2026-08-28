"""
TODO : la suite de tests complète est à construire dans le cadre de l'exercice 4.
Ce test minimal ne sert qu'à vérifier que le module s'importe et s'exécute (vert).
Objectif : couvrir les cas nominaux (paliers d'ancienneté), le bonus premium,
le plafond contractuel à 25%, et les cas limites/erreurs (montant = 0,
annees_fidelite = 0, montant négatif).
"""
from decimal import Decimal

from calculateur_remise import calculer_montant_remise


def test_cas_basique_ne_leve_pas_exception():
    resultat = calculer_montant_remise(Decimal("100"), 0, False)

    assert resultat == Decimal("0.00")
