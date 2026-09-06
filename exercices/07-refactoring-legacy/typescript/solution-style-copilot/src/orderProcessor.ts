import {
  getDiscount,
  getTax,
  needsManagerValidation,
  toCountry,
  toCustomerType,
} from "./calculators.js";

export interface Logger {
  log(message: string): void;
}

export class OrderProcessor {
  constructor(private readonly logger: Logger = console) {}

  processOrder(customerType: string, amount: number, country: string, isFirstOrder: boolean): string {
    const discount = getDiscount(toCustomerType(customerType), isFirstOrder);
    const tax = getTax(toCountry(country));
    const total = amount * (1 - discount) * (1 + tax);

    const result = needsManagerValidation(total)
      ? `Commande de ${total} EUR - necessite validation manager`
      : `Commande de ${total} EUR - validee automatiquement`;

    this.logger.log(`LOG: order processed for ${customerType} total=${total}`);

    return result;
  }
}
