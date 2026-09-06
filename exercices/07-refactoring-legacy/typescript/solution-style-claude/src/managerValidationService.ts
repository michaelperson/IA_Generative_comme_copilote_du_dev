/**
 * Décide si une commande requiert une validation manager.
 *
 * ## Gap connu, hérité du code d'origine
 *
 * Le code legacy portait le commentaire :
 *
 *     // TODO: envoyer un email au manager, jamais implémenté
 *
 * Ce comportement n'a **jamais existé**. Deux tentations, toutes deux
 * mauvaises, se présentent pendant un refactoring assisté :
 *
 * 1. Faire disparaître le TODO en même temps que le code qui l'entourait —
 *    l'information est alors perdue, et personne ne saura plus que la
 *    notification manquait.
 * 2. Implémenter l'envoi d'e-mail de sa propre initiative — on introduit un
 *    comportement nouveau que les tests de caractérisation ne détecteront pas,
 *    puisqu'ils ne le testaient pas.
 *
 * La troisième voie, retenue ici : rendre le manque explicite et bloquant si
 * quelqu'un l'appelle, pour qu'il soit spécifié avant d'être implémenté.
 */
const SEUIL_VALIDATION_MANAGER = 1000;

export class ManagerValidationService {
  requiresValidation(total: number): boolean {
    return total > SEUIL_VALIDATION_MANAGER;
  }

  notifyManager(_total: number): void {
    throw new Error(
      "Notification du manager non implémentée — comportement absent du code d'origine, " +
        "à spécifier avant toute implémentation.",
    );
  }
}
