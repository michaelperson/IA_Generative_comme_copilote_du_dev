namespace OrderProcessing;

/// <summary>
/// Responsabilité unique : calculer le taux de taxe applicable selon le pays de
/// livraison, à partir d'une table de configuration (élimine la chaîne
/// if/else-if en cascade du code legacy — principe DRY, étape 2 du refactoring).
/// </summary>
public static class TaxCalculator
{
    private const double DefaultTaxRate = 0.15;

    private static readonly Dictionary<Country, double> TaxRatesByCountry = new()
    {
        [Country.Belgium] = 0.21,
        [Country.France] = 0.20,
        [Country.Germany] = 0.19
    };

    /// <summary>
    /// Retourne le taux de taxe (ex : 0.21 pour 21%) pour le pays donné, ou
    /// <see cref="DefaultTaxRate"/> si le pays n'est pas explicitement configuré.
    /// </summary>
    public static double Calculate(Country country) =>
        TaxRatesByCountry.TryGetValue(country, out double rate) ? rate : DefaultTaxRate;
}
