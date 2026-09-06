package be.bstorm.formation.catalogue;

/**
 * Tarif catalogue.
 *
 * <p>Objet de valeur immuable : toute transformation retourne une nouvelle
 * instance. Le montant est exprimé en centimes entiers.</p>
 *
 * @param sku          référence produit.
 * @param libelle      libellé commercial.
 * @param prixCentimes prix unitaire, en centimes entiers.
 */
public record Tarif(String sku, String libelle, long prixCentimes) {
}
