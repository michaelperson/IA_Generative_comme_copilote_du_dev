namespace OrderProcessing;

/// <summary>
/// Responsabilité unique : calculer le taux de remise applicable à une commande.
/// </summary>
public static class DiscountCalculator
{
    private const double VipDiscount = 0.20;
    private const double RegularDiscount = 0.05;
    private const double FirstOrderBonus = 0.10;

    /// <summary>
    /// Calcule le taux de remise (ex : 0.20 pour 20%) pour un type de client et
    /// une éventuelle bonification de première commande.
    /// </summary>
    public static double Calculate(CustomerType customerType, bool isFirstOrder)
    {
        double discount = customerType switch
        {
            CustomerType.Vip => VipDiscount,
            CustomerType.Regular => RegularDiscount,
            _ => 0.0
        };

        if (isFirstOrder)
        {
            discount += FirstOrderBonus;
        }

        return discount;
    }
}
