namespace OrderProcessing;

/// <summary>
/// Responsabilité unique : calculer le taux de remise applicable à une commande,
/// en fonction de la catégorie du client et d'une éventuelle bonification de
/// première commande. Extrait du legacy <c>ProcessOrder</c> conformément au
/// principe de responsabilité unique (étape 1 du refactoring).
/// </summary>
public static class DiscountCalculator
{
    private const double VipDiscount = 0.20;
    private const double RegularDiscount = 0.05;
    private const double FirstOrderBonus = 0.10;

    /// <summary>
    /// Calcule le taux de remise cumulé (ex : 0.30 pour 30%) pour la catégorie de
    /// client et le statut de première commande donnés.
    /// </summary>
    /// <param name="customerType">Catégorie tarifaire du client.</param>
    /// <param name="isFirstOrder">Indique si la commande est la première du client.</param>
    /// <returns>Le taux de remise, avant application du plafond éventuel.</returns>
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
