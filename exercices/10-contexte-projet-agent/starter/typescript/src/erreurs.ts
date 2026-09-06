/** Levée lorsqu'aucun tarif ne correspond au SKU demandé. */
export class TarifIntrouvableError extends Error {
  constructor(readonly sku: string) {
    super(`Aucun tarif ne correspond au SKU « ${sku} ».`);
    this.name = "TarifIntrouvableError";
  }
}
