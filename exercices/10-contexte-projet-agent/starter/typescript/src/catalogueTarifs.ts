import { TarifIntrouvableError } from "./erreurs.js";
import { Tarif } from "./tarif.js";

/**
 * Consultation des tarifs du catalogue produit.
 *
 * Lecture et calcul uniquement : aucune persistance, aucun appel réseau.
 */
export class CatalogueTarifs {
  private readonly parSku: Map<string, Tarif>;

  constructor(tarifs: readonly Tarif[]) {
    this.parSku = new Map(tarifs.map((t) => [t.sku, t]));
  }

  /**
   * Retourne le tarif correspondant au SKU demandé.
   *
   * @param sku Référence produit recherchée.
   * @throws {TarifIntrouvableError} si aucun tarif ne correspond.
   */
  trouverParSku(sku: string): Tarif {
    const tarif = this.parSku.get(sku);
    if (tarif === undefined) throw new TarifIntrouvableError(sku);
    return tarif;
  }
}
