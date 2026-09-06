import { describe, expect, it } from "vitest";
import {
  MontantCommandeInvalideError,
  calculerMontantRemise,
} from "../src/calculateurRemise.js";

describe("calculerMontantRemise — cas nominaux", () => {
  it.each([
    [200, 0, false, 0],
    [200, 1, false, 10],
    [200, 5, false, 20],
    [200, 10, false, 30],
    [200, 0, true, 10],
    [200, 10, true, 40],
  ])(
    "montant %s, %s an(s), premium %s -> remise %s",
    (montant, annees, premium, attendu) => {
      expect(calculerMontantRemise(montant, annees, premium)).toBe(attendu);
    },
  );
});

describe("calculerMontantRemise — frontières des paliers", () => {
  it.each([
    [0, 0],
    [1, 5],
    [4, 5],
    [5, 10],
    [9, 10],
    [10, 15],
    [40, 15],
  ])("%s an(s) de fidélité -> taux de %s %%", (annees, tauxAttendu) => {
    expect(calculerMontantRemise(1000, annees, false)).toBe(1000 * (tauxAttendu / 100));
  });
});

describe("calculerMontantRemise — plafond contractuel à 25 %", () => {
  it("le taux maximal réellement atteignable est de 20 %, pas 25 %", () => {
    // Constat volontaire de l'exercice : 15 % (palier maximal) + 5 % (premium)
    // = 20 %. Le plafond de 25 % ne peut donc JAMAIS s'activer avec les
    // paliers actuels — c'est du code mort. Un test qui prétendrait atteindre
    // 25 % décrirait un scénario impossible.
    expect(calculerMontantRemise(1000, 100, true)).toBe(200);
  });
});

describe("calculerMontantRemise — cas limites", () => {
  it("un montant nul donne une remise nulle", () => {
    expect(calculerMontantRemise(0, 10, true)).toBe(0);
  });

  it("une ancienneté nulle sans premium donne une remise nulle", () => {
    expect(calculerMontantRemise(999.99, 0, false)).toBe(0);
  });

  it("arrondit « loin de zéro » un montant exactement à mi-chemin entre deux centimes", () => {
    // 0,10 € × 5 % = 0,005 € : exactement entre 0,00 et 0,01.
    // L'arrondi attendu est 0,01 (loin de zéro), pas 0,00 (arrondi bancaire).
    expect(calculerMontantRemise(0.1, 1, false)).toBe(0.01);
  });

  it("reste exact sur un montant à décimales", () => {
    expect(calculerMontantRemise(19.99, 5, false)).toBe(2);
  });
});

describe("calculerMontantRemise — cas d'erreur", () => {
  it("refuse un montant négatif avec une erreur typée", () => {
    expect(() => calculerMontantRemise(-1, 5, false)).toThrow(MontantCommandeInvalideError);
  });

  it("accepte la valeur frontière zéro sans lever d'erreur", () => {
    expect(() => calculerMontantRemise(0, 0, false)).not.toThrow();
  });
});
