/** Journalisation extraite du cœur métier, injectée par l'appelant. */
export interface OrderLogger {
  log(message: string): void;
}

export class ConsoleOrderLogger implements OrderLogger {
  log(message: string): void {
    console.log(message);
  }
}
