namespace CatalogueTarifs;

/// <summary>Levée lorsqu'aucun tarif ne correspond au SKU demandé.</summary>
public class TarifIntrouvableException : Exception
{
    /// <summary>Référence produit recherchée.</summary>
    public string Sku { get; }

    public TarifIntrouvableException(string sku)
        : base($"Aucun tarif ne correspond au SKU « {sku} ».")
    {
        Sku = sku;
    }
}
