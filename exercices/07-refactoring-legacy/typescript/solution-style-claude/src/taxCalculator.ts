import { Country } from "./types.js";

/**
 * Fournit le taux de TVA applicable.
 *
 * La table remplace la chaîne de if/else if du code d'origine. Ajouter un pays
 * devient une ligne de données au lieu d'une branche de code — c'est le
 * bénéfice concret du DRY demandé à la sous-étape 2.
 */
const TAUX_PAR_PAYS: Readonly<Record<Country, number>> = {
  [Country.Belgium]: 0.21,
  [Country.France]: 0.2,
  [Country.Germany]: 0.19,
  [Country.Other]: 0.15,
};

export class TaxCalculator {
  rateFor(country: Country): number {
    return TAUX_PAR_PAYS[country];
  }
}
