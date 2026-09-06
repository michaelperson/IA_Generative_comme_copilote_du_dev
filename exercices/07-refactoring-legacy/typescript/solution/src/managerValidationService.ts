/** Responsabilité unique : décider si la commande requiert une validation manager. */
const SEUIL_VALIDATION_MANAGER = 1000;

export class ManagerValidationService {
  requiresValidation(total: number): boolean {
    return total > SEUIL_VALIDATION_MANAGER;
  }

  /**
   * Point d'extension volontairement NON implémenté.
   *
   * Le code legacy portait le commentaire « TODO: envoyer un email au manager,
   * jamais implémenté ». Ce comportement n'a jamais existé : le refactoring
   * ne l'invente pas et ne le fait pas disparaître silencieusement — il le
   * rend visible ici, à décider explicitement.
   */
  notifyManager(_total: number): void {
    throw new Error(
      "Notification du manager non implémentée — comportement absent du code d'origine, " +
        "à spécifier avant toute implémentation.",
    );
  }
}
