package be.bstorm.formation.orderprocessing;

import java.util.Map;

/**
 * Responsabilité unique : le taux de remise. Cette classe ne connaît ni la taxe,
 * ni le seuil de validation, ni la journalisation.
 *
 * <p>Point d'attention pour la relecture : l'ordre des opérations reproduit
 * exactement celui du code d'origine — taux de base d'abord, bonus première
 * commande ensuite. En arithmétique flottante, 0.20 + 0.10 ne vaut pas
 * exactement 0.30, et inverser l'ordre changerait le résultat au dernier chiffre
 * significatif. Les tests de caractérisation le détecteraient, mais autant ne
 * pas les faire échouer pour rien.</p>
 */
public final class DiscountCalculator {

    private static final Map<CustomerType, Double> TAUX_DE_BASE = Map.of(
            CustomerType.VIP, 0.20,
            CustomerType.REGULAR, 0.05,
            CustomerType.STANDARD, 0.0);

    private static final double BONUS_PREMIERE_COMMANDE = 0.10;

    private DiscountCalculator() {
    }

    /**
     * @param customerType catégorie du client.
     * @param isFirstOrder vrai s'il s'agit de sa première commande.
     * @return le taux de remise, exprimé en fraction (0.30 pour 30 %).
     */
    public static double calculate(CustomerType customerType, boolean isFirstOrder) {
        double discount = TAUX_DE_BASE.get(customerType);
        if (isFirstOrder) {
            discount += BONUS_PREMIERE_COMMANDE;
        }
        return discount;
    }
}
