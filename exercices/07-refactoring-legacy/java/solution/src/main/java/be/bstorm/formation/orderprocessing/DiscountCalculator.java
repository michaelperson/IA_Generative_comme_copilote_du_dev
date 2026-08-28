package be.bstorm.formation.orderprocessing;

/**
 * Responsabilité unique : calculer le taux de remise applicable à une commande.
 */
public final class DiscountCalculator {

    private static final double VIP_DISCOUNT = 0.20;
    private static final double REGULAR_DISCOUNT = 0.05;
    private static final double FIRST_ORDER_BONUS = 0.10;

    private DiscountCalculator() {
    }

    /**
     * Calcule le taux de remise (ex : 0.20 pour 20%) pour un type de client et
     * une éventuelle bonification de première commande.
     */
    public static double calculate(CustomerType customerType, boolean isFirstOrder) {
        double discount = switch (customerType) {
            case VIP -> VIP_DISCOUNT;
            case REGULAR -> REGULAR_DISCOUNT;
            case STANDARD -> 0.0;
        };

        if (isFirstOrder) {
            discount += FIRST_ORDER_BONUS;
        }

        return discount;
    }
}
