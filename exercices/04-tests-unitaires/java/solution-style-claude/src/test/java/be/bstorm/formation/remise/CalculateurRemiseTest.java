package be.bstorm.formation.remise;

import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Nested;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

/**
 * Suite de tests organisée par intention plutôt que par méthode testée.
 *
 * <p>Chaque cas limite porte un commentaire expliquant <em>pourquoi</em> il est
 * testé : un test dont on ne sait plus ce qu'il protège finit par être supprimé
 * au premier refactoring qui le fait échouer.</p>
 */
class CalculateurRemiseTest {

    @Nested
    @DisplayName("Étant donné un client sans ancienneté")
    class ClientSansAnciennete {

        @Test
        @DisplayName("quand il n'est pas premium, alors aucune remise n'est accordée")
        void sansPremium() {
            assertEquals(new BigDecimal("0.00"),
                    CalculateurRemise.calculerMontantRemise(new BigDecimal("500"), 0, false));
        }

        @Test
        @DisplayName("quand il est premium, alors seul le bonus premium s'applique")
        void avecPremium() {
            assertEquals(new BigDecimal("25.00"),
                    CalculateurRemise.calculerMontantRemise(new BigDecimal("500"), 0, true));
        }
    }

    @Nested
    @DisplayName("Étant donné les frontières exactes des paliers d'ancienneté")
    class FrontieresDesPaliers {

        // Les frontières sont testées de part et d'autre : c'est là que se logent
        // les erreurs de comparaison (>= au lieu de >, et inversement).
        @ParameterizedTest(name = "{0} an(s) -> {1} EUR")
        @CsvSource({"0, 0.00", "1, 50.00", "4, 50.00", "5, 100.00", "9, 100.00", "10, 150.00"})
        void tauxParPalier(int anneesFidelite, String attendu) {
            assertEquals(new BigDecimal(attendu),
                    CalculateurRemise.calculerMontantRemise(new BigDecimal("1000"), anneesFidelite, false));
        }
    }

    @Nested
    @DisplayName("Étant donné le plafond contractuel de 25 %")
    class PlafondContractuel {

        @Test
        @DisplayName("alors il est inatteignable avec les paliers actuels — constat, pas bug à contourner")
        void plafondInatteignable() {
            // 15 % (palier maximal) + 5 % (premium) = 20 %. Le plafond de 25 % est
            // donc du code mort. Écrire un test qui prétend l'atteindre reviendrait
            // à inventer un scénario impossible : c'est le piège de cet exercice.
            // On fige plutôt le taux maximal réellement atteignable.
            assertEquals(new BigDecimal("200.00"),
                    CalculateurRemise.calculerMontantRemise(new BigDecimal("1000"), 99, true));
        }
    }

    @Nested
    @DisplayName("Étant donné les cas limites de montant")
    class CasLimitesDeMontant {

        @Test
        @DisplayName("quand le montant est nul, alors la remise est nulle")
        void montantNul() {
            assertEquals(new BigDecimal("0.00"),
                    CalculateurRemise.calculerMontantRemise(BigDecimal.ZERO, 10, true));
        }

        @Test
        @DisplayName("quand le résultat tombe entre deux centimes, alors l'arrondi se fait loin de zéro")
        void arrondiAMiChemin() {
            // 0,10 EUR x 5 % = 0,005 EUR. L'arrondi attendu est 0,01 et non 0,00 :
            // un arrondi bancaire donnerait un résultat différent, et l'écart se
            // manifesterait sur des volumes de facturation réels.
            assertEquals(new BigDecimal("0.01"),
                    CalculateurRemise.calculerMontantRemise(new BigDecimal("0.10"), 1, false));
        }

        @Test
        @DisplayName("quand le montant comporte des décimales, alors le calcul reste exact")
        void montantADecimales() {
            assertEquals(new BigDecimal("2.00"),
                    CalculateurRemise.calculerMontantRemise(new BigDecimal("19.99"), 5, false));
        }
    }

    @Nested
    @DisplayName("Étant donné un montant invalide")
    class MontantInvalide {

        @Test
        @DisplayName("quand il est négatif, alors une exception typée est levée")
        void montantNegatif() {
            // Une exception typée, et non une exception générique : l'appelant doit
            // pouvoir distinguer cette faute de saisie d'une panne technique.
            assertThrows(IllegalArgumentException.class,
                    () -> CalculateurRemise.calculerMontantRemise(new BigDecimal("-0.01"), 5, false));
        }

        @Test
        @DisplayName("quand il vaut exactement zéro, alors aucune exception n'est levée")
        void montantZero() {
            // Frontière de la garde : zéro est valide, seul le strictement négatif
            // est refusé.
            assertDoesNotThrow(
                    () -> CalculateurRemise.calculerMontantRemise(BigDecimal.ZERO, 5, false));
        }
    }
}
