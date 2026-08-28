"""Tests pour validation_tva.py.

Calcul du numéro de TVA valide utilisé ci-dessous :
    numéro d'entreprise = 12345678
    12345678 mod 97 = 3            (12345678 = 97 * 127275 + 3)
    clé de contrôle attendue = 97 - 3 = 94
    => numéro complet valide : BE1234567894
"""

from validation_tva import valider_numero_tva_belge


def test_numero_valide_doit_etre_accepte():
    resultat = valider_numero_tva_belge("BE1234567894")

    assert resultat.est_valide is True
    assert resultat.message_erreur is None


def test_cle_de_controle_fausse_doit_etre_rejetee():
    # Même numéro d'entreprise (12345678) mais clé de contrôle 93 au lieu de 94.
    resultat = valider_numero_tva_belge("BE1234567893")

    assert resultat.est_valide is False
    assert resultat.message_erreur is not None


def test_format_incorrect_doit_etre_rejete():
    resultat = valider_numero_tva_belge("FR1234567894")

    assert resultat.est_valide is False
    assert resultat.message_erreur is not None


def test_entree_vide_doit_etre_rejetee_sans_exception():
    resultat = valider_numero_tva_belge("")

    assert resultat.est_valide is False
    assert resultat.message_erreur is not None


def test_entree_none_doit_etre_rejetee_sans_exception():
    resultat = valider_numero_tva_belge(None)

    assert resultat.est_valide is False
    assert resultat.message_erreur is not None
