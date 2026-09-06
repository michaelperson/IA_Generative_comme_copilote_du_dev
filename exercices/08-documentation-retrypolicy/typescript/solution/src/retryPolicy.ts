/**
 * Politique de nouvelle tentative avec attente exponentielle, destinée à
 * fiabiliser les appels vers des services externes instables.
 *
 * Documentation de référence de l'exercice 8. La logique est volontairement
 * INCHANGÉE par rapport au code d'origine, défaut compris (voir la section
 * « Incohérence connue » et le fichier INCOHERENCES.md).
 */
export class RetryPolicy {
  /**
   * @param maxAttempts Nombre total de tentatives, la première comprise.
   *   Une valeur de 1 revient à ne pas réessayer.
   * @param baseDelayMs Délai de base en millisecondes avant la deuxième
   *   tentative. Il double à chaque tentative suivante.
   */
  constructor(
    private readonly maxAttempts: number,
    private readonly baseDelayMs: number,
  ) {}

  /**
   * Exécute l'action fournie, en la réessayant tant qu'elle échoue et que le
   * nombre maximal de tentatives n'est pas atteint.
   *
   * Pourquoi une attente exponentielle plutôt qu'un délai fixe : un service
   * externe qui échoue est le plus souvent un service déjà en difficulté
   * (surcharge, saturation, redémarrage). Réessayer à intervalle fixe ajoute
   * une charge constante et retarde son rétablissement. Doubler l'attente à
   * chaque tentative laisse au service le temps de se rétablir tout en
   * conservant une réactivité correcte sur les incidents brefs.
   *
   * Séquence pour maxAttempts = 4 et baseDelayMs = 100 : tentative, 100 ms,
   * tentative, 200 ms, tentative, 400 ms, tentative.
   *
   * @typeParam T Type de la valeur produite par l'action.
   * @param action Opération asynchrone à exécuter, potentiellement plusieurs fois.
   *   Elle doit être idempotente : rien ne garantit qu'un échec signifie que
   *   l'effet de bord n'a pas eu lieu côté serveur.
   * @returns La valeur produite par la première tentative réussie.
   * @throws {Error} Après épuisement des tentatives — voir l'incohérence connue.
   *
   * @remarks
   * **Incohérence connue.** Après épuisement des tentatives, cette méthode
   * lève `new Error("Unreachable")` au lieu de propager la dernière exception
   * rencontrée. La cause originale de l'échec est donc perdue : côté
   * exploitation, on constate un échec sans savoir s'il s'agissait d'un délai
   * dépassé, d'une erreur d'authentification ou d'une réponse 500. Ce n'est
   * pas un comportement voulu mais un défaut. Le correctif figure dans
   * `retryPolicyFixed.ts` ; il n'a pas été appliqué ici pour laisser le code
   * d'origine intact, conformément à la consigne de l'exercice.
   */
  async execute<T>(action: () => Promise<T>): Promise<T> {
    for (let attempt = 1; attempt <= this.maxAttempts; attempt++) {
      try {
        return await action();
      } catch {
        if (attempt < this.maxAttempts) {
          const delay = this.baseDelayMs * Math.pow(2, attempt - 1);
          await new Promise((r) => setTimeout(r, delay));
        }
      }
    }
    throw new Error("Unreachable");
  }
}
