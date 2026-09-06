/**
 * Variante corrigée de {@link RetryPolicy}, fournie en bonus.
 *
 * Seule différence fonctionnelle : après épuisement des tentatives, la
 * dernière exception rencontrée est propagée au lieu d'être remplacée par un
 * `Error("Unreachable")` qui masquait la cause réelle de l'échec.
 */
export class RetryPolicyFixed {
  constructor(
    private readonly maxAttempts: number,
    private readonly baseDelayMs: number,
  ) {}

  async execute<T>(action: () => Promise<T>): Promise<T> {
    let derniereErreur: unknown;

    for (let attempt = 1; attempt <= this.maxAttempts; attempt++) {
      try {
        return await action();
      } catch (error) {
        derniereErreur = error;
        if (attempt < this.maxAttempts) {
          const delay = this.baseDelayMs * Math.pow(2, attempt - 1);
          await new Promise((r) => setTimeout(r, delay));
        }
      }
    }

    throw derniereErreur;
  }
}
