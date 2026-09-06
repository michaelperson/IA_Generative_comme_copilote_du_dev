package be.bstorm.formation.remise;

import java.math.BigDecimal;
import java.math.RoundingMode;

/**
 * Calcule le montant de la remise applicable à une commande, en fonction de
 * l'ancienneté du client et de son statut premium.
 *
 * Fonction métier fournie pour l'exercice 4 (tests unitaires) : elle est déjà
 * complète et correcte. L'objectif de l'exercice est d'écrire, avec l'aide de
 * l'IA, la suite de tests unitaires qui la couvre (cf. CalculateurRemiseTest).
 */
public final class CalculateurRemise {

    private CalculateurRemise() {
    }

    /**
     * Calcule le montant de la remise (en euros), arrondi à 2 décimales
     * (arrondi "loin de zéro", {@link RoundingMode#HALF_UP}).
     *
     * @throws IllegalArgumentException si montantCommande est négatif.
     */
    public static BigDecimal calculerMontantRemise(BigDecimal montantCommande, int anneesFidelite, boolean estClientPremium) {
        if (montantCommande.signum() < 0) {
            throw new IllegalArgumentException("montantCommande doit être positif ou nul");
        }

        BigDecimal tauxBase;
        if (anneesFidelite >= 10) {
            tauxBase = new BigDecimal("0.15");
        } else if (anneesFidelite >= 5) {
            tauxBase = new BigDecimal("0.10");
        } else if (anneesFidelite >= 1) {
            tauxBase = new BigDecimal("0.05");
        } else {
            tauxBase = BigDecimal.ZERO;
        }

        BigDecimal taux = estClientPremium ? tauxBase.add(new BigDecimal("0.05")) : tauxBase;
        BigDecimal plafond = new BigDecimal("0.25");
        if (taux.compareTo(plafond) > 0) {
            taux = plafond; // plafond contractuel à 25%
        }

        return montantCommande.multiply(taux).setScale(2, RoundingMode.HALF_UP);
    }
}
