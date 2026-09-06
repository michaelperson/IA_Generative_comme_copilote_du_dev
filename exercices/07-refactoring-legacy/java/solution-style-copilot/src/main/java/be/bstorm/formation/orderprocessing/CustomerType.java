package be.bstorm.formation.orderprocessing;

/** Catégorie de client, en remplacement des chaînes magiques du code legacy. */
public enum CustomerType {
    STANDARD,
    REGULAR,
    VIP;

    public static CustomerType parse(String valeur) {
        if ("VIP".equals(valeur)) return VIP;
        if ("REGULAR".equals(valeur)) return REGULAR;
        return STANDARD;
    }
}
