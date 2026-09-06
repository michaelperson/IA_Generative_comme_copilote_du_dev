from decimal import Decimal

import pytest

from calculateur_remise import calculer_montant_remise


def test_pas_de_remise_sans_anciennete():
    assert calculer_montant_remise(Decimal("100"), 0, False) == Decimal("0.00")


def test_cinq_pour_cent_entre_1_et_4_ans():
    assert calculer_montant_remise(Decimal("100"), 3, False) == Decimal("5.00")


def test_dix_pour_cent_entre_5_et_9_ans():
    assert calculer_montant_remise(Decimal("100"), 7, False) == Decimal("10.00")


def test_quinze_pour_cent_a_partir_de_10_ans():
    assert calculer_montant_remise(Decimal("100"), 12, False) == Decimal("15.00")


def test_bonus_premium():
    assert calculer_montant_remise(Decimal("100"), 7, True) == Decimal("15.00")


def test_montant_negatif_leve_une_erreur():
    with pytest.raises(ValueError):
        calculer_montant_remise(Decimal("-10"), 1, False)
