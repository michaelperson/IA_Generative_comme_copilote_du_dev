package be.bstorm.formation.remise;

import org.junit.jupiter.api.Test;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.assertEquals;

/**
 * TODO : la suite de tests complète est à construire dans le cadre de l'exercice 4.
 * Ce test minimal ne sert qu'à vérifier que le projet compile et s'exécute (vert).
 * Objectif : couvrir les cas nominaux (paliers d'ancienneté), le bonus premium,
 * le plafond contractuel à 25%, et les cas limites/erreurs (montant = 0,
 * anneesFidelite = 0, montant négatif).
 */
class CalculateurRemiseTest {

    @Test
    void calculerMontantRemise_casBasique_neLevePasException() {
        BigDecimal resultat = CalculateurRemise.calculerMontantRemise(new BigDecimal("100"), 0, false);

        assertEquals(new BigDecimal("0.00"), resultat);
    }
}
