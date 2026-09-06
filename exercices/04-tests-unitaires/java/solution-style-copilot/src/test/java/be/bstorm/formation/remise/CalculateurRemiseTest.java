package be.bstorm.formation.remise;

import org.junit.jupiter.api.Test;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

class CalculateurRemiseTest {

    @Test
    void pasDeRemiseSansAnciennete() {
        assertEquals(new BigDecimal("0.00"),
                CalculateurRemise.calculerMontantRemise(new BigDecimal("100"), 0, false));
    }

    @Test
    void cinqPourCentEntre1Et4Ans() {
        assertEquals(new BigDecimal("5.00"),
                CalculateurRemise.calculerMontantRemise(new BigDecimal("100"), 3, false));
    }

    @Test
    void dixPourCentEntre5Et9Ans() {
        assertEquals(new BigDecimal("10.00"),
                CalculateurRemise.calculerMontantRemise(new BigDecimal("100"), 7, false));
    }

    @Test
    void quinzePourCentAPartirDe10Ans() {
        assertEquals(new BigDecimal("15.00"),
                CalculateurRemise.calculerMontantRemise(new BigDecimal("100"), 12, false));
    }

    @Test
    void bonusPremium() {
        assertEquals(new BigDecimal("15.00"),
                CalculateurRemise.calculerMontantRemise(new BigDecimal("100"), 7, true));
    }

    @Test
    void montantNegatifLeveUneErreur() {
        assertThrows(IllegalArgumentException.class,
                () -> CalculateurRemise.calculerMontantRemise(new BigDecimal("-10"), 1, false));
    }
}
