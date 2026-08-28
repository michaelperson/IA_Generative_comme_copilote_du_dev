"""
Suite de tests de référence pour calculer_montant_remise. Couvre les cas
nominaux par palier d'ancienneté, le bonus client premium, le plafond
contractuel à 25%, et les cas limites/erreurs identifiés lors de l'analyse
d'un premier jet de tests trop générique (montant = 0, annees_fidelite = 0,
montant négatif, arrondi).
"""
from decimal import Decimal

import pytest

from calculateur_remise import calculer_montant_remise


@pytest.mark.parametrize(
    "annees_fidelite, est_premium, montant_attendu",
    [
        (0, False, "0.00"),    # annees_fidelite = 0 -> aucune remise (cas limite explicitement demandé)
        (3, False, "50.00"),   # palier 1-4 ans -> 5%
        (7, False, "100.00"),  # palier 5-9 ans -> 10%
        (10, False, "150.00"), # palier >= 10 ans -> 15%
        (40, False, "150.00"), # très ancien client -> reste sur le même palier
        (0, True, "50.00"),    # 0% de base + 5% premium
        (3, True, "100.00"),   # 5% de base + 5% premium
        (7, True, "150.00"),   # 10% de base + 5% premium
        (10, True, "200.00"),  # 15% de base + 5% premium = 20% (taux réel maximal)
    ],
)
def test_cas_nominaux_et_premium(annees_fidelite, est_premium, montant_attendu):
    resultat = calculer_montant_remise(Decimal("1000"), annees_fidelite, est_premium)
    assert resultat == Decimal(montant_attendu)


@pytest.mark.parametrize(
    "annees_fidelite, montant_attendu",
    [
        (0, "0.00"),    # frontière basse du palier "0"
        (1, "50.00"),   # frontière basse du palier "1-4 ans"
        (4, "50.00"),   # frontière haute du palier "1-4 ans"
        (5, "100.00"),  # frontière basse du palier "5-9 ans"
        (9, "100.00"),  # frontière haute du palier "5-9 ans"
        (10, "150.00"), # frontière basse du palier "10 ans et plus"
    ],
)
def test_frontieres_de_palier(annees_fidelite, montant_attendu):
    # Ces frontières sont le point exact où un premier jet de tests généré trop
    # vite ("génère les tests") a tendance à ne tester qu'une valeur "typique"
    # par palier, sans vérifier les bornes (off-by-one sur les seuils 1, 5, 10).
    resultat = calculer_montant_remise(Decimal("1000"), annees_fidelite, False)
    assert resultat == Decimal(montant_attendu)


def test_plafond_25_pourcent_taux_reel_maximal_est_20_pourcent():
    # Point de vigilance identifié lors de l'itération sur le prompt (étapes 2 et 3
    # de l'exercice) : "le plafond à 25% est-il testé à sa frontière exacte ?"
    # Réponse, après analyse : avec les paliers actuels (max 15% à 10 ans) et le
    # bonus premium (+5%), le taux le plus élevé atteignable via l'API publique
    # est 20%. Le plafond de 25% ne peut donc jamais s'activer avec les entrées
    # actuelles : ce test documente ce constat plutôt que d'affirmer un scénario
    # à 25% mathématiquement inatteignable pour cette version de la fonction.
    resultat = calculer_montant_remise(Decimal("1000"), 10, True)
    assert resultat == Decimal("200.00")


def test_montant_commande_nul_la_remise_est_nulle():
    resultat = calculer_montant_remise(Decimal("0"), 10, True)
    assert resultat == Decimal("0.00")


def test_montant_commande_negatif_leve_value_error():
    with pytest.raises(ValueError):
        calculer_montant_remise(Decimal("-0.01"), 5, False)


def test_arrondi_a_mi_chemin_entre_deux_centimes_arrondit_loin_de_zero():
    # 1.3 * 5% = 0.065, exactement à mi-chemin entre 0.06 et 0.07.
    # ROUND_HALF_UP doit arrondir vers 0.07 (et non 0.06, comme le ferait un
    # arrondi bancaire ROUND_HALF_EVEN). Un premier jet de tests généré sans
    # préciser ce mode de calcul manque souvent cette nuance.
    resultat = calculer_montant_remise(Decimal("1.3"), 3, False)
    assert resultat == Decimal("0.07")
