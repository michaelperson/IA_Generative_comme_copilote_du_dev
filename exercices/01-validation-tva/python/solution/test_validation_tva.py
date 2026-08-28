"""Tests pour validation_tva.py.

Numéros de TVA valides utilisés dans ces tests :

    1) numéro d'entreprise = 12345678
       12345678 mod 97 = 3            (97 * 127275 = 12345675, reste 3)
       clé attendue = 97 - 3 = 94
       => BE1234567894

    2) numéro d'entreprise = 87654321
       87654321 mod 97 = 77           (97 * 903652 = 87654244, reste 77)
       clé attendue = 97 - 77 = 20
       => BE8765432120

    3) cas limite (modulo 97 == 0) : numéro d'entreprise = 12338400
       12338400 mod 97 = 0            (97 * 127200 = 12338400 exactement)
       => la clé "attendue" vaudrait 97 - 0 = 97, ce qui n'est pas un cas
          valide : le numéro doit être rejeté quelle que soit la clé fournie
          (ici "97").
"""

from validation_tva import valider_numero_tva_belge


def test_premier_numero_valide_doit_etre_accepte():
    resultat = valider_numero_tva_belge("BE1234567894")

    assert resultat.est_valide is True
    assert resultat.message_erreur is None


def test_second_numero_valide_doit_etre_accepte():
    resultat = valider_numero_tva_belge("BE8765432120")

    assert resultat.est_valide is True
    assert resultat.message_erreur is None


def test_numero_avec_separateurs_doit_etre_accepte_apres_nettoyage():
    resultat = valider_numero_tva_belge("BE 1234.567.894")

    assert resultat.est_valide is True
    assert resultat.message_erreur is None


def test_entree_vide_doit_etre_rejetee_sans_exception():
    resultat = valider_numero_tva_belge("")

    assert resultat.est_valide is False
    assert resultat.message_erreur is not None


def test_entree_none_doit_etre_rejetee_sans_exception():
    resultat = valider_numero_tva_belge(None)

    assert resultat.est_valide is False
    assert resultat.message_erreur is not None


def test_format_sans_prefixe_be_doit_etre_rejete():
    resultat = valider_numero_tva_belge("1234567894")

    assert resultat.est_valide is False
    assert resultat.message_erreur is not None


def test_cle_de_controle_faussee_de_un_doit_etre_rejetee():
    # Même numéro d'entreprise que le premier cas valide (12345678, clé
    # attendue 94), mais avec une clé fournie de 93 (fausse de 1).
    resultat = valider_numero_tva_belge("BE1234567893")

    assert resultat.est_valide is False
    assert resultat.message_erreur is not None


def test_numero_entreprise_multiple_de_97_doit_etre_rejete_cas_limite():
    resultat = valider_numero_tva_belge("BE1233840097")

    assert resultat.est_valide is False
    assert resultat.message_erreur is not None
