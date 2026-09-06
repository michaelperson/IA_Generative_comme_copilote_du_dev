import { PourcentageRemiseInvalideError, TarifIntrouvableError } from "./erreurs.js";
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

  /**
   * Retourne une nouvelle instance de tarif, remisée du pourcentage indiqué.
   *
   * Le calcul reste entièrement en centimes entiers : le produit
   * prix × (100 − pourcentage) est exact, et seule la division finale par 100
   * demande un arrondi, fait « loin de zéro » au centime le plus proche.
   *
   * @param sku Référence produit à remiser.
   * @param pourcentage Pourcentage de remise, entier de 0 à 100 inclus.
   * @returns Un nouveau {@link Tarif} ; le tarif d'origine est inchangé.
   * @throws {TarifIntrouvableError} si aucun tarif ne correspond au SKU.
   * @throws {PourcentageRemiseInvalideError} si le pourcentage n'est pas un
   *   entier compris entre 0 et 100.
   */
  appliquerRemise(sku: string, pourcentage: number): Tarif {
    if (!Number.isInteger(pourcentage) || pourcentage < 0 || pourcentage > 100) {
      throw new PourcentageRemiseInvalideError(pourcentage);
    }

    const tarif = this.trouverParSku(sku);

    const produit = tarif.prixCentimes * (100 - pourcentage);
    const quotient = Math.trunc(produit / 100);
    const reste = produit % 100;
    const prixRemiseCentimes = reste * 2 >= 100 ? quotient + 1 : quotient;

    return new Tarif(tarif.sku, tarif.libelle, prixRemiseCentimes);
  }
}
