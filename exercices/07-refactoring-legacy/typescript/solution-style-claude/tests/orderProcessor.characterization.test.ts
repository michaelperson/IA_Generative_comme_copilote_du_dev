import { describe, expect, it, vi } from "vitest";
import { OrderProcessor } from "../src/orderProcessor.js";

/**
 * Tests de caractérisation (étape 2 de l'exercice).
 *
 * Ils figent le comportement ACTUEL, correct ou non. Leur rôle est de
 * détecter tout changement pendant le refactoring — pas de valider que le
 * comportement est souhaitable.
 *
 * En particulier, la valeur « 114.99999999999999 » est un artefact de
 * l'arithmétique flottante du code legacy. Un test de caractérisation le fige
 * tel quel : le corriger pendant le refactoring serait un changement de
 * comportement, à décider explicitement et pas en passant.
 */
describe("OrderProcessor — caractérisation du comportement existant", () => {
  const processor = new OrderProcessor();
  const silence = () => vi.spyOn(console, "log").mockImplementation(() => {});

  it("client VIP, première commande, Belgique", () => {
    silence();
    expect(processor.processOrder("VIP", 500, "BE", true)).toBe(
      "Commande de 423.5 EUR - validee automatiquement",
    );
  });

  it("client REGULAR récurrent, France", () => {
    silence();
    expect(processor.processOrder("REGULAR", 200, "FR", false)).toBe(
      "Commande de 228 EUR - validee automatiquement",
    );
  });

  it("montant dépassant le seuil de validation manager", () => {
    silence();
    expect(processor.processOrder("VIP", 2000, "BE", false)).toBe(
      "Commande de 1936 EUR - necessite validation manager",
    );
  });

  it("type de client et pays inconnus (taux par défaut)", () => {
    silence();
    expect(processor.processOrder("UNKNOWN", 100, "US", false)).toBe(
      "Commande de 114.99999999999999 EUR - validee automatiquement",
    );
  });

  it("client REGULAR, première commande, Allemagne, au-dessus du seuil", () => {
    silence();
    expect(processor.processOrder("REGULAR", 1000, "DE", true)).toBe(
      "Commande de 1011.5 EUR - necessite validation manager",
    );
  });

  it("journalise le traitement (effet de bord existant)", () => {
    const espion = silence();
    processor.processOrder("VIP", 500, "BE", true);
    expect(espion).toHaveBeenCalledWith("LOG: order processed for VIP total=423.5");
  });
});
