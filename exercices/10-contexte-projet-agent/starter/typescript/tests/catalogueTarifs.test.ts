import { describe, expect, it } from "vitest";
import { CatalogueTarifs } from "../src/catalogueTarifs.js";
import { TarifIntrouvableError } from "../src/erreurs.js";
import { Tarif } from "../src/tarif.js";

const CATALOGUE = new CatalogueTarifs([
  new Tarif("CLA-100", "Clavier mécanique", 12990),
  new Tarif("ECR-240", "Écran 24 pouces", 19999),
  new Tarif("SOU-010", "Souris filaire", 999),
]);

describe("CatalogueTarifs.trouverParSku", () => {
  it("retourne le tarif correspondant au SKU", () => {
    expect(CATALOGUE.trouverParSku("CLA-100").prixCentimes).toBe(12990);
  });

  it("lève une erreur typée pour un SKU inconnu", () => {
    expect(() => CATALOGUE.trouverParSku("INEXISTANT")).toThrow(TarifIntrouvableError);
  });
});
