namespace CatalogueTarifs;

/// <summary>
/// Tarif catalogue.
/// </summary>
/// <remarks>
/// Objet de valeur immuable : toute transformation retourne une nouvelle
/// instance. Le montant est exprimé en centimes entiers.
/// </remarks>
/// <param name="Sku">Référence produit.</param>
/// <param name="Libelle">Libellé commercial.</param>
/// <param name="PrixCentimes">Prix unitaire, en centimes entiers.</param>
public record Tarif(string Sku, string Libelle, long PrixCentimes);
