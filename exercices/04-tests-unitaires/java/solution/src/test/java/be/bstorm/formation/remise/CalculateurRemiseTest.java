package be.bstorm.formation.remise;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

/**
 * Suite de tests de référence pour {@link CalculateurRemise#calculerMontantRemise}.
 * Couvre les cas nominaux par palier d'ancienneté, le bonus client premium,
 * le plafond contractuel à 25%, et les cas limites/erreurs identifiés lors de
 * l'analyse d'un premier jet de tests trop générique (montant = 0,
 * anneesFidelite = 0, montant négatif, arrondi).
 */
class CalculateurRemiseTest {

    // --- Cas nominaux et frontières de palier, sans puis avec le bonus premium ---

    @ParameterizedTest(name = "{0} an(s), premium={2} -> remise {3}")
    @CsvSource({
            "0,   false, 0.00",   // anneesFidelite = 0 -> aucune remise (cas limite explicitement demandé)
            "3,   false, 50.00",  // palier 1-4 ans -> 5%
            "7,   false, 100.00", // palier 5-9 ans -> 10%
            "10,  false, 150.00", // palier >= 10 ans -> 15%
            "40,  false, 150.00", // très ancien client -> reste sur le même palier
            "0,   true,  50.00",  // 0% de base + 5% premium
            "3,   true,  100.00", // 5% de base + 5% premium
            "7,   true,  150.00", // 10% de base + 5% premium
            "10,  true,  200.00", // 15% de base + 5% premium = 20% (taux réel maximal)
    })
    void calculerMontantRemise_casNominauxEtPremium_retourneLeMontantAttendu(
            int anneesFidelite, boolean estClientPremium, String montantRemiseAttendu) {
        BigDecimal resultat = CalculateurRemise.calculerMontantRemise(
                new BigDecimal("1000"), anneesFidelite, estClientPremium);

        assertEquals(new BigDecimal(montantRemiseAttendu), resultat);
    }

    @ParameterizedTest(name = "frontière à {0} an(s) -> remise {1}")
    @CsvSource({
            "0,  0.00",   // frontière basse du palier "0"
            "1,  50.00",  // frontière basse du palier "1-4 ans"
            "4,  50.00",  // frontière haute du palier "1-4 ans"
            "5,  100.00", // frontière basse du palier "5-9 ans"
            "9,  100.00", // frontière haute du palier "5-9 ans"
            "10, 150.00", // frontière basse du palier "10 ans et plus"
    })
    void calculerMontantRemise_anneesFideliteAUneFrontiereDePalier_leBonPalierEstApplique(
            int anneesFidelite, String montantRemiseAttendu) {
        // Ces frontières sont le point exact où un premier jet de tests généré trop
        // vite ("teste la fonction") a tendance à ne tester qu'une valeur "typique"
        // par palier, sans vérifier les bornes (off-by-one sur les seuils 1, 5, 10).
        BigDecimal resultat = CalculateurRemise.calculerMontantRemise(
                new BigDecimal("1000"), anneesFidelite, false);

        assertEquals(new BigDecimal(montantRemiseAttendu), resultat);
    }

    // --- Plafond contractuel à 25% ---

    @Test
    void calculerMontantRemise_meilleureCombinaisonPossible_leTauxReelPlafonneNaturellementA20Pourcent() {
        // Point de vigilance identifié lors de l'itération sur le prompt (étapes 2 et 3
        // de l'exercice) : "le plafond à 25% est-il testé à sa frontière exacte ?"
        // Réponse, après analyse : avec les paliers actuels (max 15% à 10 ans) et le
        // bonus premium (+5%), le taux le plus élevé atteignable via l'API publique
        // est 20%. Le plafond de 25% ne peut donc jamais s'activer avec les entrées
        // actuelles : ce test documente ce constat plutôt que d'affirmer un scénario
        // à 25% mathématiquement inatteignable pour cette version de la fonction.
        BigDecimal resultat = CalculateurRemise.calculerMontantRemise(
                new BigDecimal("1000"), 10, true);

        assertEquals(new BigDecimal("200.00"), resultat);
    }

    // --- Cas limites explicitement demandés dans la consigne ---

    @Test
    void calculerMontantRemise_montantCommandeNul_laRemiseEstNulle() {
        BigDecimal resultat = CalculateurRemise.calculerMontantRemise(BigDecimal.ZERO, 10, true);

        assertEquals(new BigDecimal("0.00"), resultat);
    }

    @Test
    void calculerMontantRemise_montantCommandeNegatif_leveIllegalArgumentException() {
        assertThrows(IllegalArgumentException.class, () ->
                CalculateurRemise.calculerMontantRemise(new BigDecimal("-0.01"), 5, false));
    }

    // --- Comportement de l'arrondi ---

    @Test
    void calculerMontantRemise_montantExactementAMiCheminEntreDeuxCentimes_arrondiLoinDeZero() {
        // 1.3 * 5% = 0.065, exactement à mi-chemin entre 0.06 et 0.07.
        // RoundingMode.HALF_UP (équivalent Java de AwayFromZero pour les valeurs
        // positives) doit arrondir vers 0.07, et non 0.06 (arrondi bancaire).
        BigDecimal resultat = CalculateurRemise.calculerMontantRemise(
                new BigDecimal("1.3"), 3, false);

        assertEquals(new BigDecimal("0.07"), resultat);
    }
}
