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

describe("CatalogueTarifs.appliquerRemise", () => {
  it("applique une remise et retourne un nouveau tarif", () => {
    const remise = CATALOGUE.appliquerRemise("CLA-100", 10);
    expect(remise.prixCentimes).toBe(11691);
    expect(remise.sku).toBe("CLA-100");
    expect(remise.libelle).toBe("Clavier mécanique");
  });

  it("laisse le tarif d'origine inchangé", () => {
    CATALOGUE.appliquerRemise("ECR-240", 25);
    expect(CATALOGUE.trouverParSku("ECR-240").prixCentimes).toBe(19999);
  });

  it("accepte les bornes 0 et 100", () => {
    expect(CATALOGUE.appliquerRemise("SOU-010", 0).prixCentimes).toBe(999);
    expect(CATALOGUE.appliquerRemise("SOU-010", 100).prixCentimes).toBe(0);
  });

  it("arrondit au centime le plus proche, loin de zéro", () => {
    // 999 x 95 = 94905 ; 94905 / 100 = 949,05 -> 949
    expect(CATALOGUE.appliquerRemise("SOU-010", 5).prixCentimes).toBe(949);
    // 19999 x 75 = 1499925 ; /100 = 14999,25 -> 14999
    expect(CATALOGUE.appliquerRemise("ECR-240", 25).prixCentimes).toBe(14999);
    // 999 x 50 = 49950 ; /100 = 499,50 -> 500 (loin de zéro, pas 499)
    expect(CATALOGUE.appliquerRemise("SOU-010", 50).prixCentimes).toBe(500);
  });

  it.each([-1, 101, 10.5])("refuse un pourcentage invalide (%s)", (p) => {
    expect(() => CATALOGUE.appliquerRemise("CLA-100", p)).toThrow(
      /pourcentage de remise/i,
    );
  });

  it("propage l'erreur de SKU inconnu", () => {
    expect(() => CATALOGUE.appliquerRemise("INEXISTANT", 10)).toThrow(TarifIntrouvableError);
  });
});
