package be.bstorm.formation.orderprocessing;

import java.util.Map;

public final class Calculators {

    private static final Map<CustomerType, Double> DISCOUNTS = Map.of(
            CustomerType.VIP, 0.20,
            CustomerType.REGULAR, 0.05,
            CustomerType.STANDARD, 0.0);

    private static final Map<Country, Double> TAXES = Map.of(
            Country.BELGIUM, 0.21,
            Country.FRANCE, 0.20,
            Country.GERMANY, 0.19,
            Country.OTHER, 0.15);

    private static final double FIRST_ORDER_BONUS = 0.10;
    private static final double MANAGER_THRESHOLD = 1000;

    private Calculators() {
    }

    public static double discount(CustomerType type, boolean isFirstOrder) {
        double d = DISCOUNTS.get(type);
        if (isFirstOrder) d += FIRST_ORDER_BONUS;
        return d;
    }

    public static double tax(Country country) {
        return TAXES.get(country);
    }

    public static boolean needsManagerValidation(double total) {
        return total > MANAGER_THRESHOLD;
    }
}
