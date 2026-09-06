import { describe, expect, it } from "vitest";
import { validerNumeroTvaBelge } from "../src/validationTva.js";

describe("validerNumeroTvaBelge", () => {
  it("accepte un numéro valide sans séparateur", () => {
    // 01234567 % 97 = 48 -> clé attendue = 49
    const r = validerNumeroTvaBelge("BE0123456749");
    expect(r.estValide).toBe(true);
    expect(r.messageErreur).toBeNull();
  });

  it("accepte le même numéro avec espaces, points et tirets", () => {
    expect(validerNumeroTvaBelge("BE 0123.456-749").estValide).toBe(true);
  });

  it("accepte une saisie en minuscules", () => {
    expect(validerNumeroTvaBelge("be0123456749").estValide).toBe(true);
  });

  it("rejette une clé de contrôle fausse et le dit", () => {
    const r = validerNumeroTvaBelge("BE0123456748");
    expect(r.estValide).toBe(false);
    expect(r.messageErreur).toContain("Clé de contrôle invalide");
  });

  it("rejette un format incorrect", () => {
    const r = validerNumeroTvaBelge("FR0123456749");
    expect(r.estValide).toBe(false);
    expect(r.messageErreur).toContain("Format invalide");
  });

  it("rejette un numéro trop court", () => {
    expect(validerNumeroTvaBelge("BE012345674").estValide).toBe(false);
  });

  it.each([null, undefined, "", "   "])("rejette l'entrée vide (%s)", (entree) => {
    const r = validerNumeroTvaBelge(entree as string | null | undefined);
    expect(r.estValide).toBe(false);
    expect(r.messageErreur).toContain("vide");
  });

  it("rejette le cas limite du modulo 97 (numéro d'entreprise multiple de 97)", () => {
    // 00000097 % 97 = 0 -> cas limite, invalide quelle que soit la clé
    const r = validerNumeroTvaBelge("BE0000009700");
    expect(r.estValide).toBe(false);
    expect(r.messageErreur).toContain("modulo 97");
  });
});
