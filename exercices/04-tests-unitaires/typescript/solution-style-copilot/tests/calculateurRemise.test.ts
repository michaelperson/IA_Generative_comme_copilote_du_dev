import { describe, expect, it } from "vitest";
import {
  MontantCommandeInvalideError,
  calculerMontantRemise,
} from "../src/calculateurRemise.js";

describe("calculerMontantRemise", () => {
  it("pas de remise sans ancienneté", () => {
    expect(calculerMontantRemise(100, 0, false)).toBe(0);
  });

  it("5 % entre 1 et 4 ans", () => {
    expect(calculerMontantRemise(100, 3, false)).toBe(5);
  });

  it("10 % entre 5 et 9 ans", () => {
    expect(calculerMontantRemise(100, 7, false)).toBe(10);
  });

  it("15 % à partir de 10 ans", () => {
    expect(calculerMontantRemise(100, 12, false)).toBe(15);
  });

  it("bonus premium de 5 points", () => {
    expect(calculerMontantRemise(100, 7, true)).toBe(15);
  });

  it("lève une erreur sur montant négatif", () => {
    expect(() => calculerMontantRemise(-10, 1, false)).toThrow(MontantCommandeInvalideError);
  });
});
