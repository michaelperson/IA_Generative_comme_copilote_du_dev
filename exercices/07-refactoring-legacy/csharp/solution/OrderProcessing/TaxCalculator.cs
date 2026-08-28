namespace OrderProcessing;

/// <summary>
/// Responsabilité unique : calculer le taux de taxe applicable selon le pays,
/// à partir d'un dictionnaire de configuration (élimine la chaîne de if/else legacy).
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
    /// Retourne le taux de taxe (ex : 0.21 pour 21%) pour le pays donné,
    /// ou le taux par défaut si le pays n'est pas dans la table.
    /// </summary>
    public static double Calculate(Country country) =>
        TaxRatesByCountry.TryGetValue(country, out double rate) ? rate : DefaultTaxRate;
}
