"""
Tests de caractérisation (étape 2 de l'exercice) : ils figent le comportement
observé du OrderProcessor AVANT refactoring, pour au moins les 3 scénarios
demandés dans le prompt étape 2. Ils s'exécutent ici contre l'OrderProcessor
refactoré (solution) sans avoir été modifiés (étape 4) : la signature
publique et les valeurs de retour sont restées identiques au comportement
legacy d'origine.
"""
from order_processor import OrderProcessor


def test_client_vip_premiere_commande_belgique_est_validee_automatiquement():
    processor = OrderProcessor()

    result = processor.process_order("VIP", 1000, "BE", True)

    assert result == "Commande de 847.0 EUR - validee automatiquement"


def test_client_regulier_recurrent_france_est_validee_automatiquement():
    processor = OrderProcessor()

    result = processor.process_order("REGULAR", 500, "FR", False)

    assert result == "Commande de 570.0 EUR - validee automatiquement"


def test_montant_depassant_le_seuil_necessite_validation_manager():
    processor = OrderProcessor()

    result = processor.process_order("REGULAR", 2000, "DE", False)

    assert result == "Commande de 2261.0 EUR - necessite validation manager"
