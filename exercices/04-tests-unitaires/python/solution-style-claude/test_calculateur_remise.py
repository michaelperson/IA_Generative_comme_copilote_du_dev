"""Suite de tests organisée par intention plutôt que par méthode testée.

Chaque cas limite porte un commentaire expliquant POURQUOI il est testé : un
test dont on ne sait plus ce qu'il protège finit par être supprimé au premier
refactoring qui le fait échouer.
"""

from decimal import Decimal

import pytest

from calculateur_remise import calculer_montant_remise


class TestClientSansAnciennete:
    def test_quand_non_premium_alors_aucune_remise(self):
        assert calculer_montant_remise(Decimal("500"), 0, False) == Decimal("0.00")

    def test_quand_premium_alors_seul_le_bonus_sapplique(self):
        assert calculer_montant_remise(Decimal("500"), 0, True) == Decimal("25.00")


class TestFrontieresDesPaliers:
    # Les frontières sont testées de part et d'autre : c'est là que se logent
    # les erreurs de comparaison (>= au lieu de >, et inversement).
    @pytest.mark.parametrize(
        "annees, attendu",
        [(0, "0.00"), (1, "50.00"), (4, "50.00"), (5, "100.00"), (9, "100.00"), (10, "150.00")],
    )
    def test_taux_par_palier(self, annees, attendu):
        assert calculer_montant_remise(Decimal("1000"), annees, False) == Decimal(attendu)


class TestPlafondContractuel:
    def test_le_plafond_de_25_pourcent_est_inatteignable(self):
        # 15 % (palier maximal) + 5 % (premium) = 20 %. Le plafond de 25 % est
        # donc du code mort. Écrire un test qui prétend l'atteindre reviendrait
        # à inventer un scénario impossible — c'est le piège de cet exercice.
        # On fige plutôt le taux maximal réellement atteignable.
        assert calculer_montant_remise(Decimal("1000"), 99, True) == Decimal("200.00")


class TestCasLimitesDeMontant:
    def test_montant_nul_donne_remise_nulle(self):
        assert calculer_montant_remise(Decimal("0"), 10, True) == Decimal("0.00")

    def test_arrondi_loin_de_zero_a_mi_chemin_entre_deux_centimes(self):
        # 0,10 € x 5 % = 0,005 €. L'arrondi attendu est 0,01 € et non 0,00 € :
        # un arrondi bancaire donnerait un résultat différent, et l'écart se
        # manifesterait sur des volumes de facturation réels.
        assert calculer_montant_remise(Decimal("0.10"), 1, False) == Decimal("0.01")

    def test_montant_a_decimales_reste_exact(self):
        assert calculer_montant_remise(Decimal("19.99"), 5, False) == Decimal("2.00")


class TestMontantInvalide:
    def test_montant_negatif_leve_une_erreur_typee(self):
        # Une erreur explicite, et non un retour silencieux : l'appelant doit
        # pouvoir distinguer cette faute de saisie d'une panne technique.
        with pytest.raises(ValueError):
            calculer_montant_remise(Decimal("-0.01"), 5, False)

    def test_zero_est_accepte(self):
        # Frontière de la garde : zéro est valide, seul le strictement négatif
        # est refusé.
        calculer_montant_remise(Decimal("0"), 5, False)
