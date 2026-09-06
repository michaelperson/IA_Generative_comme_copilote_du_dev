import { Country } from "./country.js";

/** Responsabilité unique : fournir le taux de taxe applicable au pays. */
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
