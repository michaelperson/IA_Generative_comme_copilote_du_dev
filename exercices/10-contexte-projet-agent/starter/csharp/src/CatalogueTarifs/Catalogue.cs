namespace CatalogueTarifs;

/// <summary>
/// Consultation des tarifs du catalogue produit.
/// </summary>
/// <remarks>
/// Lecture et calcul uniquement : aucune persistance, aucun appel réseau.
/// </remarks>
public class Catalogue
{
    private readonly Dictionary<string, Tarif> _parSku;

    public Catalogue(IEnumerable<Tarif> tarifs)
    {
        _parSku = tarifs.ToDictionary(t => t.Sku);
    }

    /// <summary>Retourne le tarif correspondant au SKU demandé.</summary>
    /// <param name="sku">Référence produit recherchée.</param>
    /// <returns>Le tarif correspondant.</returns>
    /// <exception cref="TarifIntrouvableException">Si aucun tarif ne correspond.</exception>
    public Tarif TrouverParSku(string sku)
    {
        if (!_parSku.TryGetValue(sku, out var tarif))
        {
            throw new TarifIntrouvableException(sku);
        }

        return tarif;
    }
}
