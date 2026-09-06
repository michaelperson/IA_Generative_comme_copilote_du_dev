/**
 * Version corrigée de l'exercice 6.
 *
 * Principe de la correction, identique dans les quatre langages du dépôt :
 * ne jamais mettre en cache une entité suivie par l'ORM, mais un instantané
 * immuable, et toujours recharger l'entité depuis la session courante avant
 * toute écriture.
 */

import { OrderStatus, Session } from "./models.js";

/** Instantané immuable, sans lien avec une session de persistance. */
export interface OrderSnapshot {
  readonly id: string;
  readonly status: OrderStatus;
}

/** Cache applicatif partagé — ne contient que des instantanés détachés. */
const cache = new Map<string, OrderSnapshot>();

/** Lecture seule : sert le cache s'il est chaud, sans jamais rattacher d'entité. */
export async function getOrderSnapshot(
  session: Session,
  orderId: string,
): Promise<OrderSnapshot> {
  const enCache = cache.get(orderId);
  if (enCache !== undefined) return enCache;

  const order = await session.find(orderId);
  const snapshot: OrderSnapshot = { id: order.id, status: order.status };
  cache.set(orderId, snapshot);
  return snapshot;
}

/**
 * Écriture : recharge systématiquement l'entité depuis la session courante.
 * Le cache est invalidé puis réalimenté avec le nouvel instantané.
 */
export async function updateOrderStatus(
  session: Session,
  orderId: string,
  newStatus: OrderStatus,
): Promise<void> {
  const order = await session.find(orderId);
  order.status = newStatus;
  await session.saveChanges();
  cache.set(orderId, { id: order.id, status: newStatus });
}

export function viderCache(): void {
  cache.clear();
}
