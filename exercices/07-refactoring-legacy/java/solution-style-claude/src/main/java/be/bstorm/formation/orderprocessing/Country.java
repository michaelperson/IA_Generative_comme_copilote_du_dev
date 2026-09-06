package be.bstorm.formation.orderprocessing;

/** Pays géré par le calcul de taxe. */
public enum Country {
    BELGIUM,
    FRANCE,
    GERMANY,
    OTHER;

    public static Country parse(String valeur) {
        if ("BE".equals(valeur)) return BELGIUM;
        if ("FR".equals(valeur)) return FRANCE;
        if ("DE".equals(valeur)) return GERMANY;
        return OTHER;
    }
}
