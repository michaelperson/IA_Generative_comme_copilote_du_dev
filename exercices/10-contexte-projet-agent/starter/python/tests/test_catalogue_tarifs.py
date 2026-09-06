import pytest

from catalogue_tarifs import CatalogueTarifs
from erreurs import TarifIntrouvableError
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
