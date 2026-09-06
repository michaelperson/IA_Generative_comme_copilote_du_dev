/**
 * Code fautif de l'exercice 6 : il reproduit l'incident décrit dans ERROR.txt.
 *
 * Ne le corrigez pas avant d'avoir suivi les trois étapes du README
 * (diagnostic à l'aveugle, puis diagnostic contextualisé, puis correction).
 */

import { Order, OrderStatus, Session } from "./models.js";

/** Cache applicatif partagé entre toutes les requêtes. */
const cache = new Map<string, Order>();

export async function updateOrderStatus(
  session: Session,
  orderId: string,
  newStatus: OrderStatus,
): Promise<void> {
  let order = cache.get(orderId);
  if (order === undefined) {
    order = await session.find(orderId);
    cache.set(orderId, order);
  }

  order.status = newStatus;
  session.track(order); // ligne 25
  await session.saveChanges();
}

export function viderCache(): void {
  cache.clear();
}
