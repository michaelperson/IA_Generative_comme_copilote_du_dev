namespace OrderProcessing;

/// <summary>
/// Version refactorée du traitement de commande legacy (voir README de l'exercice
/// pour la consigne complète en 4 étapes nommées).
/// </summary>
/// <remarks>
/// La signature publique de <see cref="ProcessOrder"/> est volontairement inchangée
/// (mêmes paramètres, même valeur de retour) afin que les tests de caractérisation
/// écrits à l'étape 2 continuent de passer sans modification (étape 4). Le calcul de
/// la remise (<see cref="DiscountCalculator"/>), de la taxe (<see cref="TaxCalculator"/>)
/// et la décision de validation manager (<see cref="ManagerValidationService"/>) sont
/// désormais délégués à des classes dédiées à responsabilité unique (étape 1), les
/// chaînes magiques sont remplacées par des enums (étape 2), et la journalisation
/// passe par une abstraction injectée (étape 3).
/// </remarks>
public class OrderProcessor
{
    private readonly ILogger _logger;
    private readonly ManagerValidationService _managerValidationService;

    /// <summary>Construit un processeur utilisant le logger console par défaut.</summary>
    public OrderProcessor() : this(new ConsoleLogger())
    {
    }

    /// <summary>Construit un processeur avec un logger injecté (utile pour les tests).</summary>
    public OrderProcessor(ILogger logger)
    {
        _logger = logger;
        _managerValidationService = new ManagerValidationService();
    }

    /// <summary>
    /// Traite une commande et retourne le message de validation correspondant.
    /// </summary>
    /// <param name="customerType">Catégorie client legacy ("VIP", "REGULAR", ou autre).</param>
    /// <param name="amount">Montant hors remise et hors taxe de la commande.</param>
    /// <param name="country">Code pays legacy ("BE", "FR", "DE", ou autre).</param>
    /// <param name="isFirstOrder">Indique si c'est la première commande du client.</param>
    /// <returns>Un message décrivant le total calculé et le statut de validation.</returns>
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
            // Gap connu, conservé à dessein : voir ManagerValidationService.NotifyManagerForValidation.
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
