namespace OrderProcessing;

public static class DiscountCalculator
{
    public static double Calculate(CustomerType customerType, bool isFirstOrder)
    {
        double discount = 0;
        if (customerType == CustomerType.Vip) discount = 0.20;
        else if (customerType == CustomerType.Regular) discount = 0.05;

        if (isFirstOrder) discount += 0.10;

        return discount;
    }
}

public static class TaxCalculator
{
    private static readonly Dictionary<Country, double> Rates = new()
    {
        [Country.Belgium] = 0.21,
        [Country.France] = 0.20,
        [Country.Germany] = 0.19
    };

    public static double Calculate(Country country)
    {
        return Rates.TryGetValue(country, out double rate) ? rate : 0.15;
    }
}
