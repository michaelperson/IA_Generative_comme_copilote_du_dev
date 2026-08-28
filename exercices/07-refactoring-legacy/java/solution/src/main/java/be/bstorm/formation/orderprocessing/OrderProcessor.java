package be.bstorm.formation.orderprocessing;

/**
 * Version refactorée du traitement de commande legacy.
 * La signature publique de {@link #processOrder} est volontairement inchangée
 * (mêmes paramètres, même valeur de retour) afin que les tests de caractérisation
 * écrits à l'étape 2 continuent de passer sans être modifiés (étape 4).
 * Le calcul de la remise, de la taxe et la décision de validation manager sont
 * désormais délégués à des classes dédiées (principe de responsabilité unique).
 */
public class OrderProcessor {

    private final OrderLogger logger;
    private final ManagerValidationService managerValidationService;

    public OrderProcessor() {
        this(new ConsoleOrderLogger());
    }

    public OrderProcessor(OrderLogger logger) {
        this.logger = logger;
        this.managerValidationService = new ManagerValidationService();
    }

    /** Traite une commande et retourne le message de validation correspondant. */
    public String processOrder(String customerType, double amount, String country, boolean isFirstOrder) {
        CustomerType customerTypeEnum = CustomerType.parse(customerType);
        Country countryEnum = Country.parse(country);

        double discount = DiscountCalculator.calculate(customerTypeEnum, isFirstOrder);
        double tax = TaxCalculator.calculate(countryEnum);

        double total = amount * (1 - discount) * (1 + tax);

        String result;
        if (managerValidationService.requiresManagerValidation(total)) {
            // TODO (toujours pas implémenté, voir ManagerValidationService) : notifier réellement le manager.
            managerValidationService.notifyManagerForValidation(total);
            result = "Commande de " + total + " EUR - necessite validation manager";
        } else {
            result = "Commande de " + total + " EUR - validee automatiquement";
        }

        logger.log("LOG: order processed for " + customerType + " total=" + total);

        return result;
    }
}
