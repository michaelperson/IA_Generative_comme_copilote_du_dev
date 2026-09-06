// TODO (exercice 8) : cette classe n'a aucune documentation. C'est l'objet de
// l'exercice : utilisez le prompt du README pour générer une documentation
// professionnelle (TSDoc) qui explique le POURQUOI du backoff exponentiel et
// pas seulement le QUOI, et qui signale toute incohérence détectée dans le
// code ci-dessous.
//
// Ne modifiez pas la logique : l'exercice porte sur la documentation d'un code
// existant, pas sur sa correction.

export class RetryPolicy {
  constructor(
    private readonly maxAttempts: number,
    private readonly baseDelayMs: number,
  ) {}

  async execute<T>(action: () => Promise<T>): Promise<T> {
    for (let attempt = 1; attempt <= this.maxAttempts; attempt++) {
      try {
        return await action();
      } catch {
        if (attempt < this.maxAttempts) {
          const delay = this.baseDelayMs * Math.pow(2, attempt - 1);
          await new Promise((r) => setTimeout(r, delay));
        }
      }
    }
    throw new Error("Unreachable");
  }
}
