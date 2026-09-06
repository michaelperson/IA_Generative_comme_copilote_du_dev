import pytest

from catalogue_tarifs import CatalogueTarifs
from erreurs import PourcentageRemiseInvalideError, TarifIntrouvableError
from tarif import Tarif

CATALOGUE = CatalogueTarifs(
    [
        Tarif("CLA-100", "Clavier mécanique", 12990),
        Tarif("ECR-240", "Écran 24 pouces", 19999),
        Tarif("SOU-010", "Souris filaire", 999),
    ]
)


def test_trouver_par_sku_retourne_le_tarif():
    assert CATALOGUE.trouver_par_sku("CLA-100").prix_centimes == 12990


def test_trouver_par_sku_leve_une_erreur_typee():
    with pytest.raises(TarifIntrouvableError):
        CATALOGUE.trouver_par_sku("INEXISTANT")


def test_appliquer_remise_retourne_un_nouveau_tarif():
    remise = CATALOGUE.appliquer_remise("CLA-100", 10)
    assert remise.prix_centimes == 11691
    assert remise.sku == "CLA-100"
    assert remise.libelle == "Clavier mécanique"


def test_le_tarif_dorigine_est_inchange():
    CATALOGUE.appliquer_remise("ECR-240", 25)
    assert CATALOGUE.trouver_par_sku("ECR-240").prix_centimes == 19999


def test_bornes_0_et_100_acceptees():
    assert CATALOGUE.appliquer_remise("SOU-010", 0).prix_centimes == 999
    assert CATALOGUE.appliquer_remise("SOU-010", 100).prix_centimes == 0


def test_arrondi_au_centime_loin_de_zero():
    # 999 x 95 = 94905 ; /100 = 949,05 -> 949
    assert CATALOGUE.appliquer_remise("SOU-010", 5).prix_centimes == 949
    # 19999 x 75 = 1499925 ; /100 = 14999,25 -> 14999
    assert CATALOGUE.appliquer_remise("ECR-240", 25).prix_centimes == 14999
    # 999 x 50 = 49950 ; /100 = 499,50 -> 500 (loin de zéro, pas 499)
    assert CATALOGUE.appliquer_remise("SOU-010", 50).prix_centimes == 500


@pytest.mark.parametrize("pourcentage", [-1, 101, 10.5])
def test_pourcentage_invalide_refuse(pourcentage):
    with pytest.raises(PourcentageRemiseInvalideError):
        CATALOGUE.appliquer_remise("CLA-100", pourcentage)


def test_sku_inconnu_propage_lerreur():
    with pytest.raises(TarifIntrouvableError):
        CATALOGUE.appliquer_remise("INEXISTANT", 10)
