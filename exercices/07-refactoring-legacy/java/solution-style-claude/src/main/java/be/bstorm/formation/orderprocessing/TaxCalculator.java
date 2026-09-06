package be.bstorm.formation.orderprocessing;

import java.util.Map;

/**
 * Fournit le taux de TVA applicable.
 *
 * <p>La table remplace la chaîne de {@code if / else if} du code d'origine.
 * Ajouter un pays devient une ligne de données au lieu d'une branche de code :
 * c'est le bénéfice concret du DRY demandé à la sous-étape 2 de l'exercice.</p>
 */
public final class TaxCalculator {

    private static final Map<Country, Double> TAUX_PAR_PAYS = Map.of(
            Country.BELGIUM, 0.21,
            Country.FRANCE, 0.20,
            Country.GERMANY, 0.19,
            Country.OTHER, 0.15);

    private TaxCalculator() {
    }

    public static double calculate(Country country) {
        return TAUX_PAR_PAYS.get(country);
    }
}
