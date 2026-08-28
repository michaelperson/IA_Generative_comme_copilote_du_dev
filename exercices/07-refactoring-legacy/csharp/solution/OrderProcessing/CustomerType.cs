namespace OrderProcessing;

/// <summary>
/// Catégories de client utilisées pour le calcul de la remise.
/// Remplace les chaînes magiques "VIP" / "REGULAR" du code legacy.
/// </summary>
public enum CustomerType
{
    Standard,
    Regular,
    Vip
}

/// <summary>
/// Traduit la valeur texte legacy (customerType) vers l'enum <see cref="CustomerType"/>,
/// afin de garder l'API publique de <see cref="OrderProcessor"/> inchangée pour les tests
/// de caractérisation existants.
/// </summary>
public static class CustomerTypeParser
{
    public static CustomerType Parse(string customerType) => customerType switch
    {
        "VIP" => CustomerType.Vip,
        "REGULAR" => CustomerType.Regular,
        _ => CustomerType.Standard
    };
}
