package be.bstorm.formation.tva;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertNull;
import static org.junit.jupiter.api.Assertions.assertTrue;

/*
 * Numéros de TVA valides utilisés dans ces tests :
 *
 *   1) numéro d'entreprise = 12345678
 *      12345678 mod 97 = 3            (97 * 127275 = 12345675, reste 3)
 *      clé attendue = 97 - 3 = 94
 *      => BE1234567894
 *
 *   2) numéro d'entreprise = 87654321
 *      87654321 mod 97 = 77           (97 * 903652 = 87654244, reste 77)
 *      clé attendue = 97 - 77 = 20
 *      => BE8765432120
 *
 *   3) cas limite (modulo 97 == 0) : numéro d'entreprise = 12338400
 *      12338400 mod 97 = 0            (97 * 127200 = 12338400 exactement)
 *      => la clé "attendue" vaudrait 97 - 0 = 97, ce qui n'est pas un cas valide :
 *         le numéro doit être rejeté quelle que soit la clé fournie (ici "97").
 */
class ValidateurTvaTest {

    @Test
    void premierNumeroValide_doitEtreAccepte() {
        ResultatValidationTva resultat = ValidateurTva.validerNumeroTvaBelge("BE1234567894");

        assertTrue(resultat.estValide());
        assertNull(resultat.messageErreur());
    }

    @Test
    void secondNumeroValide_doitEtreAccepte() {
        ResultatValidationTva resultat = ValidateurTva.validerNumeroTvaBelge("BE8765432120");

        assertTrue(resultat.estValide());
        assertNull(resultat.messageErreur());
    }

    @Test
    void numeroAvecSeparateurs_doitEtreAccepteApresNettoyage() {
        ResultatValidationTva resultat = ValidateurTva.validerNumeroTvaBelge("BE 1234.567.894");

        assertTrue(resultat.estValide());
        assertNull(resultat.messageErreur());
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

    @Test
    void formatSansPrefixeBe_doitEtreRejete() {
        ResultatValidationTva resultat = ValidateurTva.validerNumeroTvaBelge("1234567894");

        assertFalse(resultat.estValide());
        assertNotNull(resultat.messageErreur());
    }

    @Test
    void cleDeControleFausseeDeUn_doitEtreRejetee() {
        // Même numéro d'entreprise que le premier cas valide (12345678, clé attendue 94),
        // mais avec une clé fournie de 93 (fausse de 1).
        ResultatValidationTva resultat = ValidateurTva.validerNumeroTvaBelge("BE1234567893");

        assertFalse(resultat.estValide());
        assertNotNull(resultat.messageErreur());
    }

    @Test
    void numeroEntrepriseMultipleDe97_doitEtreRejeteCasLimite() {
        ResultatValidationTva resultat = ValidateurTva.validerNumeroTvaBelge("BE1233840097");

        assertFalse(resultat.estValide());
        assertNotNull(resultat.messageErreur());
    }
}
