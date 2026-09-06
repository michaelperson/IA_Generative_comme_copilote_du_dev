/** Types de client reconnus. Remplace les chaînes magiques du code legacy. */
export enum CustomerType {
  Vip = "VIP",
  Regular = "REGULAR",
  Unknown = "UNKNOWN",
}

export function parseCustomerType(valeur: string): CustomerType {
  return valeur === CustomerType.Vip || valeur === CustomerType.Regular
    ? (valeur as CustomerType)
    : CustomerType.Unknown;
}
