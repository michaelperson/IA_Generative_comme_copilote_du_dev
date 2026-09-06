import { describe, expect, it } from "vitest";
import { RetryPolicy } from "../src/retryPolicy.js";
import { RetryPolicyFixed } from "../src/retryPolicyFixed.js";

class ErreurReseau extends Error {
  constructor() {
    super("connexion refusée par le service de paiement");
    this.name = "ErreurReseau";
  }
}

describe("RetryPolicy", () => {
  it("retourne la valeur dès la première tentative réussie", async () => {
    let appels = 0;
    const politique = new RetryPolicy(3, 1);
    const valeur = await politique.execute(async () => {
      appels += 1;
      return "ok";
    });
    expect(valeur).toBe("ok");
    expect(appels).toBe(1);
  });

  it("réessaie puis réussit", async () => {
    let appels = 0;
    const politique = new RetryPolicy(3, 1);
    const valeur = await politique.execute(async () => {
      appels += 1;
      if (appels < 3) throw new ErreurReseau();
      return "ok";
    });
    expect(valeur).toBe("ok");
    expect(appels).toBe(3);
  });

  it("perd la cause originale après épuisement des tentatives (défaut documenté)", async () => {
    const politique = new RetryPolicy(2, 1);
    await expect(
      politique.execute(async () => {
        throw new ErreurReseau();
      }),
    ).rejects.toThrow("Unreachable");
  });
});

describe("RetryPolicyFixed", () => {
  it("propage la dernière exception rencontrée", async () => {
    const politique = new RetryPolicyFixed(2, 1);
    await expect(
      politique.execute(async () => {
        throw new ErreurReseau();
      }),
    ).rejects.toBeInstanceOf(ErreurReseau);
  });
});
