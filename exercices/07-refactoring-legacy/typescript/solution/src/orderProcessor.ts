import { Country, parseCountry } from "./country.js";
import { CustomerType, parseCustomerType } from "./customerType.js";
import { DiscountCalculator } from "./discountCalculator.js";
import { ManagerValidationService } from "./managerValidationService.js";
import { ConsoleOrderLogger, type OrderLogger } from "./orderLogger.js";
import { TaxCalculator } from "./taxCalculator.js";

/**
 * Version refactorée selon les quatre étapes nommées de l'exercice :
 * responsabilité unique, élimination des chaînes magiques et de la
 * duplication, extraction de l'effet de bord de journalisation, et
 * vérification par les tests de caractérisation, inchangés.
 *
 * Le comportement observable est strictement identique à celui du code
 * d'origine, artefacts d'arithmétique flottante compris.
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
    const type: CustomerType = parseCustomerType(customerType);
    const pays: Country = parseCountry(country);

    const discount = this.discounts.calculate(type, isFirstOrder);
    const tax = this.taxes.rateFor(pays);
    const total = amount * (1 - discount) * (1 + tax);

    const result = this.validation.requiresValidation(total)
      ? `Commande de ${total} EUR - necessite validation manager`
      : `Commande de ${total} EUR - validee automatiquement`;

    this.logger.log(`LOG: order processed for ${customerType} total=${total}`);

    return result;
  }
}
