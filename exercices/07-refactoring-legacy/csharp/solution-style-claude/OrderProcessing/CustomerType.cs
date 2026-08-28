namespace OrderProcessing;

/// <summary>
/// Catégorie tarifaire du client, utilisée par <see cref="DiscountCalculator"/> pour
/// déterminer la remise applicable.
/// </summary>
/// <remarks>
/// Remplace les chaînes magiques "VIP" / "REGULAR" du code legacy d'origine : le
/// compilateur garantit désormais qu'aucune valeur inattendue ne peut circuler dans le
/// domaine métier.
/// </remarks>
public enum CustomerType
{
    /// <summary>Client sans statut particulier (aucune remise catégorielle).</summary>
    Standard,

    /// <summary>Client fidélisé bénéficiant d'une remise réduite.</summary>
    Regular,

    /// <summary>Client à statut préférentiel bénéficiant de la remise la plus élevée.</summary>
    Vip
}

/// <summary>
/// Convertit la valeur texte historique du champ <c>customerType</c> en
/// <see cref="CustomerType"/>, afin que la signature publique de
/// <see cref="OrderProcessor.ProcessOrder"/> reste inchangée pour les appelants
/// existants et pour les tests de caractérisation.
/// </summary>
public static class CustomerTypeParser
{
    /// <summary>
    /// Traduit la chaîne legacy en <see cref="CustomerType"/>. Toute valeur inconnue
    /// (y compris <c>null</c> ou vide) retombe sur <see cref="CustomerType.Standard"/>,
    /// exactement comme le faisait implicitement le code d'origine (branche "else").
    /// </summary>
    public static CustomerType Parse(string? customerType) => customerType switch
    {
        "VIP" => CustomerType.Vip,
        "REGULAR" => CustomerType.Regular,
        _ => CustomerType.Standard
    };
}
