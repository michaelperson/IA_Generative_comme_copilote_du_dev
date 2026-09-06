import { describe, expect, it } from "vitest";
import { calculerMontantRemise } from "../src/calculateurRemise.js";

/**
 * TODO (exercice 4) : ce fichier ne contient qu'UN test minimal, volontairement.
 *
 * Étape 1 — demandez à votre assistant « Génère les tests unitaires pour cette
 *           fonction » et notez le nombre de cas couverts.
 * Étape 2 — analysez ce qui manque (frontières des paliers, plafond à 25 %,
 *           montant nul, montant négatif, arrondi à mi-chemin).
 * Étape 3 — renvoyez un prompt de relance ciblé listant les cas manquants.
 */
describe("calculerMontantRemise", () => {
  it("applique 10 % à un client de 5 ans non premium", () => {
    expect(calculerMontantRemise(200, 5, false)).toBe(20);
  });
});
