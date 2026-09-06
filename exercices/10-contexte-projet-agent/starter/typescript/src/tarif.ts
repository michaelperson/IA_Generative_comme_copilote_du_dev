/**
 * Tarif catalogue.
 *
 * Objet de valeur immuable : toute transformation retourne une nouvelle
 * instance. Le montant est exprimé en centimes entiers.
 */
export class Tarif {
  constructor(
    readonly sku: string,
    readonly libelle: string,
    readonly prixCentimes: number,
  ) {
    Object.freeze(this);
  }
}
