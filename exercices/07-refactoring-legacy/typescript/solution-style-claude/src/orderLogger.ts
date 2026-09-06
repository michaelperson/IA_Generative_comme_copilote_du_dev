/**
 * Interface de journalisation injectée.
 *
 * Le code d'origine appelait console.log directement depuis le cœur métier.
 * Deux conséquences : impossible de tester la méthode sans polluer la sortie,
 * et impossible de rediriger les traces vers l'infrastructure de logs réelle
 * sans modifier la logique métier.
 */
export interface OrderLogger {
  log(message: string): void;
}

export class ConsoleOrderLogger implements OrderLogger {
  log(message: string): void {
    console.log(message);
  }
}
