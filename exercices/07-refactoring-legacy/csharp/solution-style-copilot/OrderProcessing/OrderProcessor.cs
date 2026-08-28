namespace OrderProcessing;

public class OrderProcessor
{
    private readonly ILogger _logger;

    public OrderProcessor() : this(new ConsoleLogger())
    {
    }

    public OrderProcessor(ILogger logger)
    {
        _logger = logger;
    }

    public string ProcessOrder(string customerType, double amount, string country, bool isFirstOrder)
    {
        var type = CustomerTypeParser.Parse(customerType);
        var countryEnum = CountryParser.Parse(country);

        double discount = DiscountCalculator.Calculate(type, isFirstOrder);
        double tax = TaxCalculator.Calculate(countryEnum);

        double total = amount * (1 - discount) * (1 + tax);

        string result;
        if (ManagerValidation.IsRequired(total))
        {
            ManagerValidation.Notify(total);
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
