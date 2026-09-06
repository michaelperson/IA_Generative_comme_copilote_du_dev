import { CustomerType } from "./customerType.js";

/**
 * Responsabilité unique : calculer le taux de remise.
 *
 * L'ordre des opérations reproduit exactement celui du code legacy
 * (taux de base, puis ajout du bonus première commande) afin de préserver le
 * résultat au bit près — les tests de caractérisation le vérifient.
 */
const TAUX_DE_BASE: Readonly<Record<CustomerType, number>> = {
  [CustomerType.Vip]: 0.2,
  [CustomerType.Regular]: 0.05,
  [CustomerType.Unknown]: 0,
};

const BONUS_PREMIERE_COMMANDE = 0.1;

export class DiscountCalculator {
  calculate(customerType: CustomerType, isFirstOrder: boolean): number {
    let discount = TAUX_DE_BASE[customerType];
    if (isFirstOrder) discount += BONUS_PREMIERE_COMMANDE;
    return discount;
  }
}
