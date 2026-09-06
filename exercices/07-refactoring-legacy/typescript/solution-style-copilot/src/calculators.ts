export enum CustomerType {
  Vip = "VIP",
  Regular = "REGULAR",
  Unknown = "UNKNOWN",
}

export enum Country {
  Belgium = "BE",
  France = "FR",
  Germany = "DE",
  Other = "OTHER",
}

const DISCOUNTS: Record<CustomerType, number> = {
  [CustomerType.Vip]: 0.2,
  [CustomerType.Regular]: 0.05,
  [CustomerType.Unknown]: 0,
};

const TAXES: Record<Country, number> = {
  [Country.Belgium]: 0.21,
  [Country.France]: 0.2,
  [Country.Germany]: 0.19,
  [Country.Other]: 0.15,
};

const FIRST_ORDER_BONUS = 0.1;
const MANAGER_THRESHOLD = 1000;

export function toCustomerType(v: string): CustomerType {
  return v === "VIP" || v === "REGULAR" ? (v as CustomerType) : CustomerType.Unknown;
}

export function toCountry(v: string): Country {
  return v === "BE" || v === "FR" || v === "DE" ? (v as Country) : Country.Other;
}

export function getDiscount(type: CustomerType, isFirstOrder: boolean): number {
  let d = DISCOUNTS[type];
  if (isFirstOrder) d += FIRST_ORDER_BONUS;
  return d;
}

export function getTax(country: Country): number {
  return TAXES[country];
}

export function needsManagerValidation(total: number): boolean {
  return total > MANAGER_THRESHOLD;
}
