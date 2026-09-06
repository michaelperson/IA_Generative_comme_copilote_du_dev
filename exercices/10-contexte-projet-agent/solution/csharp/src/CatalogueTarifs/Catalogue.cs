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

    /// <summary>
    /// Retourne une nouvelle instance de tarif, remisée du pourcentage indiqué.
    /// </summary>
    /// <remarks>
    /// Le calcul reste entièrement en centimes entiers : le produit
    /// prix × (100 − pourcentage) est exact, et seule la division finale par
    /// 100 demande un arrondi, fait « loin de zéro » au centime le plus proche.
    /// </remarks>
    /// <param name="sku">Référence produit à remiser.</param>
    /// <param name="pourcentage">Pourcentage de remise, entier de 0 à 100 inclus.</param>
    /// <returns>Un nouveau <see cref="Tarif"/> ; le tarif d'origine est inchangé.</returns>
    /// <exception cref="TarifIntrouvableException">Si aucun tarif ne correspond au SKU.</exception>
    /// <exception cref="PourcentageRemiseInvalideException">
    /// Si le pourcentage sort de l'intervalle autorisé.
    /// </exception>
    public Tarif AppliquerRemise(string sku, int pourcentage)
    {
        if (pourcentage < 0 || pourcentage > 100)
        {
            throw new PourcentageRemiseInvalideException(pourcentage);
        }

        var tarif = TrouverParSku(sku);

        long produit = tarif.PrixCentimes * (100L - pourcentage);
        long quotient = produit / 100;
        long reste = produit % 100;
        long prixRemiseCentimes = reste * 2 >= 100 ? quotient + 1 : quotient;

        return tarif with { PrixCentimes = prixRemiseCentimes };
    }
}
