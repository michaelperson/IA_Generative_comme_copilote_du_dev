package be.bstorm.formation.orderprocessing;

public class OrderProcessor {

    private final OrderLogger logger;

    public OrderProcessor() {
        this(new ConsoleOrderLogger());
    }

    public OrderProcessor(OrderLogger logger) {
        this.logger = logger;
    }

    public String processOrder(String customerType, double amount, String country, boolean isFirstOrder) {
        double discount = Calculators.discount(CustomerType.parse(customerType), isFirstOrder);
        double tax = Calculators.tax(Country.parse(country));
        double total = amount * (1 - discount) * (1 + tax);

        String result = Calculators.needsManagerValidation(total)
                ? "Commande de " + total + " EUR - necessite validation manager"
                : "Commande de " + total + " EUR - validee automatiquement";

        logger.log("LOG: order processed for " + customerType + " total=" + total);

        return result;
    }
}
