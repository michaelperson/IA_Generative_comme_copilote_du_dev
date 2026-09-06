/** Levée lorsqu'aucun tarif ne correspond au SKU demandé. */
export class TarifIntrouvableError extends Error {
  constructor(readonly sku: string) {
    super(`Aucun tarif ne correspond au SKU « ${sku} ».`);
    this.name = "TarifIntrouvableError";
  }
}

/** Levée lorsque le pourcentage de remise sort de l'intervalle autorisé. */
export class PourcentageRemiseInvalideError extends Error {
  constructor(readonly pourcentage: number) {
    super(`Le pourcentage de remise doit être un entier de 0 à 100, reçu ${pourcentage}.`);
    this.name = "PourcentageRemiseInvalideError";
  }
}
