package be.bstorm.formation.catalogue;

/** Levée lorsqu'aucun tarif ne correspond au SKU demandé. */
public class TarifIntrouvableException extends RuntimeException {

    private final String sku;

    public TarifIntrouvableException(String sku) {
        super("Aucun tarif ne correspond au SKU « " + sku + " ».");
        this.sku = sku;
    }

    public String getSku() {
        return sku;
    }
}
