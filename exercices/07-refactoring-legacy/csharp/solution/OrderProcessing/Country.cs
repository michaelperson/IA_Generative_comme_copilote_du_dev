namespace OrderProcessing;

/// <summary>
/// Pays gérés par le calcul de taxe. Remplace les chaînes magiques "BE" / "FR" / "DE"
/// du code legacy.
/// </summary>
public enum Country
{
    Belgium,
    France,
    Germany,
    Other
}

/// <summary>
/// Traduit le code pays texte legacy (country) vers l'enum <see cref="Country"/>.
/// </summary>
public static class CountryParser
{
    public static Country Parse(string country) => country switch
    {
        "BE" => Country.Belgium,
        "FR" => Country.France,
        "DE" => Country.Germany,
        _ => Country.Other
    };
}
