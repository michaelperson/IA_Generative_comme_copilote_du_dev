package be.bstorm.formation.tva;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertNull;
import static org.junit.jupiter.api.Assertions.assertTrue;

/*
 * Calcul du numéro de TVA valide utilisé ci-dessous :
 *   numéro d'entreprise = 12345678
 *   12345678 mod 97 = 3            (12345678 = 97 * 127275 + 3)
 *   clé de contrôle attendue = 97 - 3 = 94
 *   => numéro complet valide : BE1234567894
 */
class ValidateurTvaTest {

    @Test
    void numeroValide_doitEtreAccepte() {
        ResultatValidationTva resultat = ValidateurTva.validerNumeroTvaBelge("BE1234567894");

        assertTrue(resultat.estValide());
        assertNull(resultat.messageErreur());
    }

    @Test
    void cleDeControleFausse_doitEtreRejetee() {
        // Même numéro d'entreprise (12345678) mais clé de contrôle 93 au lieu de 94.
        ResultatValidationTva resultat = ValidateurTva.validerNumeroTvaBelge("BE1234567893");

        assertFalse(resultat.estValide());
        assertNotNull(resultat.messageErreur());
    }

    @Test
    void formatIncorrect_doitEtreRejete() {
        ResultatValidationTva resultat = ValidateurTva.validerNumeroTvaBelge("FR1234567894");

        assertFalse(resultat.estValide());
        assertNotNull(resultat.messageErreur());
    }

    @Test
    void entreeVide_doitEtreRejeteeSansException() {
        ResultatValidationTva resultat = ValidateurTva.validerNumeroTvaBelge("");

        assertFalse(resultat.estValide());
        assertNotNull(resultat.messageErreur());
    }

    @Test
    void entreeNull_doitEtreRejeteeSansException() {
        ResultatValidationTva resultat = ValidateurTva.validerNumeroTvaBelge(null);

        assertFalse(resultat.estValide());
        assertNotNull(resultat.messageErreur());
    }
}
