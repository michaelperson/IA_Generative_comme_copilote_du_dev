package be.bstorm.formation.orderprocessing;

import java.util.Map;

/**
 * Responsabilité unique : calculer le taux de taxe applicable selon le pays,
 * à partir d'une table de configuration (élimine la chaîne de if/else legacy).
 */
public final class TaxCalculator {

    private static final double DEFAULT_TAX_RATE = 0.15;

    private static final Map<Country, Double> TAX_RATES_BY_COUNTRY = Map.of(
            Country.BELGIUM, 0.21,
            Country.FRANCE, 0.20,
            Country.GERMANY, 0.19
    );

    private TaxCalculator() {
    }

    /**
     * Retourne le taux de taxe (ex : 0.21 pour 21%) pour le pays donné, ou
     * le taux par défaut si le pays n'est pas dans la table.
     */
    public static double calculate(Country country) {
        return TAX_RATES_BY_COUNTRY.getOrDefault(country, DEFAULT_TAX_RATE);
    }
}
