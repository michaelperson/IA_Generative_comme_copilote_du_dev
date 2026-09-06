import { CustomerType } from "./types.js";

/**
 * Calcul du taux de remise. Responsabilité unique : cette classe ne connaît ni
 * la taxe, ni le seuil de validation, ni la journalisation.
 *
 * Point d'attention pour la relecture : l'ordre des opérations reproduit
 * exactement celui du code d'origine — taux de base d'abord, bonus première
 * commande ensuite. En arithmétique flottante, 0.2 + 0.1 ne vaut pas
 * exactement 0.30, et inverser l'ordre changerait le résultat au dernier
 * chiffre significatif. Les tests de caractérisation le détecteraient, mais
 * autant ne pas les faire échouer pour rien.
 */
const TAUX_DE_BASE: Readonly<Record<CustomerType, number>> = {
  [CustomerType.Vip]: 0.2,
  [CustomerType.Regular]: 0.05,
  [CustomerType.Unknown]: 0,
};

const BONUS_PREMIERE_COMMANDE = 0.1;

export class DiscountCalculator {
  /**
   * @param customerType Catégorie du client.
   * @param isFirstOrder true s'il s'agit de sa première commande.
   * @returns Le taux de remise, exprimé en fraction (0.30 pour 30 %).
   */
  calculate(customerType: CustomerType, isFirstOrder: boolean): number {
    let discount = TAUX_DE_BASE[customerType];
    if (isFirstOrder) discount += BONUS_PREMIERE_COMMANDE;
    return discount;
  }
}
