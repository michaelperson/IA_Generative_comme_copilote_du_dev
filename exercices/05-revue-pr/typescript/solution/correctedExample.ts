/**
 * Version corrigée du code de la Pull Request, appliquant les cinq remarques
 * de la revue (voir REVIEW.md).
 */

import { setTimeout as sleep } from "node:timers/promises";

// Dépendance externe supprimée : Node fournit fetch nativement depuis la v18,
// node-fetch n'apportait rien ici (remarque n° 4 de la revue).

const MAX_RETRY_ATTEMPTS = 3;
const RETRY_DELAY_MS = 30_000;

export interface Logger {
  error(contexte: Record<string, unknown>, message: string): void;
}

export class OrderService {
  constructor(
    private readonly baseUrl: string,
    private readonly logger: Logger,
    private readonly fetchFn: typeof fetch = fetch,
  ) {}

  /**
   * Récupère les commandes en attente et retourne le nombre traité.
   *
   * @param signal Permet à l'appelant d'annuler l'opération, y compris
   *   pendant l'attente entre deux tentatives.
   */
  async fetchAndProcessOrders(signal?: AbortSignal): Promise<number> {
    let processedCount = 0;

    for (let attempt = 1; attempt <= MAX_RETRY_ATTEMPTS; attempt++) {
      signal?.throwIfAborted();

      try {
        const response = await this.fetchFn(`${this.baseUrl}/orders/pending`, { signal });
        const orders = (await response.json()) as unknown[];
        processedCount += orders.length;
        return processedCount;
      } catch (err) {
        if (signal?.aborted) throw err;
        this.logger.error(
          { err, attempt, maxAttempts: MAX_RETRY_ATTEMPTS },
          "Échec de récupération des commandes en attente",
        );
      }

      if (attempt < MAX_RETRY_ATTEMPTS) {
        await sleep(RETRY_DELAY_MS, undefined, { signal });
      }
    }

    return processedCount;
  }
}
