/** Pays reconnus pour le calcul de la taxe. */
export enum Country {
  Belgium = "BE",
  France = "FR",
  Germany = "DE",
  Other = "OTHER",
}

export function parseCountry(valeur: string): Country {
  return valeur === Country.Belgium || valeur === Country.France || valeur === Country.Germany
    ? (valeur as Country)
    : Country.Other;
}
