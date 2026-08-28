package be.bstorm.formation.orderprocessing;

/**
 * Pays gérés par le calcul de taxe. Remplace les chaînes magiques
 * "BE" / "FR" / "DE" du code legacy.
 */
public enum Country {
    BELGIUM,
    FRANCE,
    GERMANY,
    OTHER;

    /** Traduit le code pays texte legacy (country) vers {@link Country}. */
    public static Country parse(String country) {
        if ("BE".equals(country)) {
            return BELGIUM;
        }
        if ("FR".equals(country)) {
            return FRANCE;
        }
        if ("DE".equals(country)) {
            return GERMANY;
        }
        return OTHER;
    }
}
