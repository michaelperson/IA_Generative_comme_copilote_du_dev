namespace OrderProcessing;

/// <summary>
/// Pays pris en charge par <see cref="TaxCalculator"/>. Remplace les chaînes
/// magiques "BE" / "FR" / "DE" du code legacy d'origine.
/// </summary>
public enum Country
{
    /// <summary>Belgique — taux de taxe le plus élevé des pays explicitement gérés (21%).</summary>
    Belgium,

    /// <summary>France (20%).</summary>
    France,

    /// <summary>Allemagne (19%).</summary>
    Germany,

    /// <summary>Tout autre pays, retombant sur le taux par défaut (15%).</summary>
    Other
}

/// <summary>
/// Convertit le code pays texte historique en <see cref="Country"/>, afin que la
/// signature publique de <see cref="OrderProcessor.ProcessOrder"/> reste inchangée.
/// </summary>
public static class CountryParser
{
    /// <summary>
    /// Traduit le code pays legacy en <see cref="Country"/>. Toute valeur non reconnue
    /// retombe sur <see cref="Country.Other"/>, exactement comme le faisait
    /// implicitement le code d'origine (branche "else").
    /// </summary>
    public static Country Parse(string? country) => country switch
    {
        "BE" => Country.Belgium,
        "FR" => Country.France,
        "DE" => Country.Germany,
        _ => Country.Other
    };
}
