package be.bstorm.formation.orderprocessing;

/**
 * Catégories de client utilisées pour le calcul de la remise.
 * Remplace les chaînes magiques "VIP" / "REGULAR" du code legacy.
 */
public enum CustomerType {
    STANDARD,
    REGULAR,
    VIP;

    /**
     * Traduit la valeur texte legacy (customerType) vers {@link CustomerType},
     * afin de garder l'API publique de {@link OrderProcessor} inchangée pour
     * les tests de caractérisation existants.
     */
    public static CustomerType parse(String customerType) {
        if ("VIP".equals(customerType)) {
            return VIP;
        }
        if ("REGULAR".equals(customerType)) {
            return REGULAR;
        }
        return STANDARD;
    }
}
