namespace OrderProcessing;

/// <summary>
/// Version refactorée du traitement de commande legacy.
/// La signature publique de <see cref="ProcessOrder"/> est volontairement inchangée
/// (mêmes paramètres, même valeur de retour) afin que les tests de caractérisation
/// écrits à l'étape 2 continuent de passer sans être modifiés (étape 4).
/// Le calcul de la remise, de la taxe et la décision de validation manager sont
/// désormais délégués à des classes dédiées (principe de responsabilité unique).
/// </summary>
public class OrderProcessor
{
    private readonly ILogger _logger;
    private readonly ManagerValidationService _managerValidationService;

    public OrderProcessor() : this(new ConsoleLogger())
    {
    }

    public OrderProcessor(ILogger logger)
    {
        _logger = logger;
        _managerValidationService = new ManagerValidationService();
    }

    /// <summary>
    /// Traite une commande et retourne le message de validation correspondant.
    /// </summary>
    public string ProcessOrder(string customerType, double amount, string country, bool isFirstOrder)
    {
        CustomerType customerTypeEnum = CustomerTypeParser.Parse(customerType);
        Country countryEnum = CountryParser.Parse(country);

        double discount = DiscountCalculator.Calculate(customerTypeEnum, isFirstOrder);
        double tax = TaxCalculator.Calculate(countryEnum);

        double total = amount * (1 - discount) * (1 + tax);

        string result;
        if (_managerValidationService.RequiresManagerValidation(total))
        {
            // TODO (toujours pas implémenté, voir ManagerValidationService) : notifier réellement le manager.
            _managerValidationService.NotifyManagerForValidation(total);
            result = "Commande de " + total + " EUR - necessite validation manager";
        }
        else
        {
            result = "Commande de " + total + " EUR - validee automatiquement";
        }

        _logger.Log("LOG: order processed for " + customerType + " total=" + total);

        return result;
    }
}
