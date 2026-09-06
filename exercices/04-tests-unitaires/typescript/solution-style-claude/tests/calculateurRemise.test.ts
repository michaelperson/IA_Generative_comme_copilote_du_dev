import { describe, expect, it } from "vitest";
import {
  MontantCommandeInvalideError,
  calculerMontantRemise,
} from "../src/calculateurRemise.js";

/**
 * Suite de tests organisée par intention plutôt que par méthode testée.
 *
 * Chaque cas limite porte un commentaire expliquant POURQUOI il est testé :
 * un test dont on ne sait plus ce qu'il protège finit par être supprimé au
 * premier refactoring qui le fait échouer.
 */
describe("calculerMontantRemise", () => {
  describe("étant donné un client sans ancienneté", () => {
    it("quand il n'est pas premium, alors aucune remise n'est accordée", () => {
      expect(calculerMontantRemise(500, 0, false)).toBe(0);
    });

    it("quand il est premium, alors seul le bonus premium s'applique", () => {
      expect(calculerMontantRemise(500, 0, true)).toBe(25);
    });
  });

  describe("étant donné les frontières exactes des paliers d'ancienneté", () => {
    // Les frontières sont testées de part et d'autre : c'est là que se logent
    // les erreurs de comparaison (>= au lieu de >, et inversement).
    it.each([
      [0, 0],
      [1, 50],
      [4, 50],
      [5, 100],
      [9, 100],
      [10, 150],
    ])("quand l'ancienneté vaut %s an(s), alors la remise vaut %s €", (annees, attendu) => {
      expect(calculerMontantRemise(1000, annees, false)).toBe(attendu);
    });
  });

  describe("étant donné le plafond contractuel de 25 %", () => {
    it("alors il est inatteignable avec les paliers actuels, et c'est un constat, pas un bug à contourner", () => {
      // 15 % (palier maximal) + 5 % (premium) = 20 %. Le plafond de 25 % est
      // donc du code mort. Écrire un test qui prétend l'atteindre reviendrait
      // à inventer un scénario impossible — c'est exactement le piège de cet
      // exercice. On fige plutôt le taux maximal réellement atteignable.
      expect(calculerMontantRemise(1000, 99, true)).toBe(200);
    });
  });

  describe("étant donné les cas limites de montant", () => {
    it("quand le montant est nul, alors la remise est nulle", () => {
      expect(calculerMontantRemise(0, 10, true)).toBe(0);
    });

    it("quand le résultat tombe exactement entre deux centimes, alors l'arrondi se fait loin de zéro", () => {
      // 0,10 € × 5 % = 0,005 €. L'arrondi attendu est 0,01 € et non 0,00 € :
      // un arrondi bancaire donnerait un résultat différent, et l'écart se
      // manifesterait sur des volumes de facturation réels.
      expect(calculerMontantRemise(0.1, 1, false)).toBe(0.01);
    });

    it("quand le montant comporte des décimales, alors le calcul reste exact", () => {
      expect(calculerMontantRemise(19.99, 5, false)).toBe(2);
    });
  });

  describe("étant donné un montant invalide", () => {
    it("quand il est négatif, alors une erreur typée est levée", () => {
      // Une erreur typée, et non une erreur générique : l'appelant doit
      // pouvoir distinguer cette faute de saisie d'une panne technique.
      expect(() => calculerMontantRemise(-0.01, 5, false)).toThrow(MontantCommandeInvalideError);
    });

    it("quand il vaut exactement zéro, alors aucune erreur n'est levée", () => {
      // Frontière de la garde : zéro est valide, seul le strictement négatif
      // est refusé.
      expect(() => calculerMontantRemise(0, 5, false)).not.toThrow();
    });
  });
});
