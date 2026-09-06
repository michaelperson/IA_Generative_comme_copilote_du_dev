package be.bstorm.formation.orderprocessing;

/**
 * Orchestration du traitement d'une commande.
 *
 * <p>Après refactoring, cette classe ne fait plus que coordonner : elle ne
 * calcule rien elle-même. C'est le signe que la responsabilité unique est
 * réellement appliquée, et pas seulement que le code a été découpé.</p>
 *
 * <p>La signature publique est inchangée : les appelants existants n'ont rien à
 * modifier, et les tests de caractérisation passent sans avoir été touchés.</p>
 */
public class OrderProcessor {

    private final OrderLogger logger;
    private final ManagerValidationService validation;

    public OrderProcessor() {
        this(new ConsoleOrderLogger(), new ManagerValidationService());
    }

    /** Constructeur de commodité : journalisation injectée, validation par défaut. */
    public OrderProcessor(OrderLogger logger) {
        this(logger, new ManagerValidationService());
    }

    public OrderProcessor(OrderLogger logger, ManagerValidationService validation) {
        this.logger = logger;
        this.validation = validation;
    }

    public String processOrder(String customerType, double amount, String country, boolean isFirstOrder) {
        double discount = DiscountCalculator.calculate(CustomerType.parse(customerType), isFirstOrder);
        double tax = TaxCalculator.calculate(Country.parse(country));
        double total = amount * (1 - discount) * (1 + tax);

        String result = validation.requiresValidation(total)
                ? "Commande de " + total + " EUR - necessite validation manager"
                : "Commande de " + total + " EUR - validee automatiquement";

        logger.log("LOG: order processed for " + customerType + " total=" + total);

        return result;
    }
}
