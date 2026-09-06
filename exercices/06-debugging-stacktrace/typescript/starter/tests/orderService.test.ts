import { beforeEach, describe, expect, it } from "vitest";
import { OrderStatus, Session, lireStatutEnBase } from "../src/models.js";
import { updateOrderStatus, viderCache } from "../src/orderService.js";

const ORDER_ID = "11111111-1111-1111-1111-111111111111";

describe("updateOrderStatus", () => {
  beforeEach(() => viderCache());

  it("traite deux requêtes successives avec des sessions distinctes", async () => {
    // Reproduit l'incident : chaque requête HTTP a sa propre session.
    await updateOrderStatus(new Session(), ORDER_ID, OrderStatus.Paid);
    await updateOrderStatus(new Session(), ORDER_ID, OrderStatus.Shipped);
    expect(lireStatutEnBase(ORDER_ID)).toBe(OrderStatus.Shipped);
  });

  it("traite plusieurs requêtes concurrentes", async () => {
    await Promise.all([
      updateOrderStatus(new Session(), ORDER_ID, OrderStatus.Paid),
      updateOrderStatus(new Session(), ORDER_ID, OrderStatus.Paid),
    ]);
    expect(lireStatutEnBase(ORDER_ID)).toBe(OrderStatus.Paid);
  });
});
