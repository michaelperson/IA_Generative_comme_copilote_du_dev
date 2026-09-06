import { DiscountCalculator } from "./discountCalculator.js";
import { ManagerValidationService } from "./managerValidationService.js";
import { ConsoleOrderLogger, type OrderLogger } from "./orderLogger.js";
import { TaxCalculator } from "./taxCalculator.js";
import { parseCountry, parseCustomerType } from "./types.js";

/**
 * Orchestration du traitement d'une commande.
 *
 * Après refactoring, cette classe ne fait plus que coordonner : elle ne
 * calcule rien elle-même. C'est le signe que la responsabilité unique est
 * réellement appliquée, et pas seulement que le code a été découpé.
 *
 * La signature publique est inchangée : les appelants existants n'ont rien à
 * modifier, et les tests de caractérisation passent sans avoir été touchés.
 */
export class OrderProcessor {
  constructor(
    private readonly discounts = new DiscountCalculator(),
    private readonly taxes = new TaxCalculator(),
    private readonly validation = new ManagerValidationService(),
    private readonly logger: OrderLogger = new ConsoleOrderLogger(),
  ) {}

  processOrder(
    customerType: string,
    amount: number,
    country: string,
    isFirstOrder: boolean,
  ): string {
    const discount = this.discounts.calculate(parseCustomerType(customerType), isFirstOrder);
    const tax = this.taxes.rateFor(parseCountry(country));
    const total = amount * (1 - discount) * (1 + tax);

    const result = this.validation.requiresValidation(total)
      ? `Commande de ${total} EUR - necessite validation manager`
      : `Commande de ${total} EUR - validee automatiquement`;

    this.logger.log(`LOG: order processed for ${customerType} total=${total}`);

    return result;
  }
}
