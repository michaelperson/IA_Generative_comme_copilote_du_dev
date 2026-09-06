/** Code proposé dans la Pull Request : récupération et traitement des commandes en attente. */

import { setTimeout as sleep } from "node:timers/promises";
import fetch from "node-fetch";

export class OrderService {
  constructor(private readonly baseUrl: string) {}

  // Récupère les commandes en attente depuis l'API et les traite
  async fetchAndProcessOrders(): Promise<number> {
    let resultat = 0;

    for (let i = 0; i < 3; i++) {
      try {
        const response = await fetch(`${this.baseUrl}/orders/pending`);
        const orders = (await response.json()) as unknown[];
        resultat += orders.length;
        return resultat;
      } catch {
        // rien
      }

      await sleep(30000);
    }

    return resultat;
  }
}
