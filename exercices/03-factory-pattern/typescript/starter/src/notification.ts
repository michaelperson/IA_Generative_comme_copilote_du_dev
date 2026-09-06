/**
 * Exercice 3 — Factory Method appliqué à une fabrique de notifications.
 *
 * TODO (exercice 3) : compléter ce fichier pour obtenir une VRAIE application
 * du Factory Method — un créateur abstrait avec une méthode de fabrication
 * abstraite, surchargée par un créateur concret par canal.
 *
 * Piège de l'exercice : un simple switch qui instancie directement les classes
 * concrètes n'est PAS un Factory Method. Si votre assistant vous en propose un,
 * relancez-le en nommant explicitement la contrainte.
 */

export interface Notification {
  send(message: string): string;
}

export class EmailNotification implements Notification {
  send(message: string): string {
    return `[Email] envoyé : ${message}`;
  }
}

export class SmsNotification implements Notification {
  send(message: string): string {
    return `[SMS] envoyé : ${message}`;
  }
}

export class PushNotification implements Notification {
  send(message: string): string {
    return `[Push] envoyé : ${message}`;
  }
}

export enum NotificationChannel {
  Email = "EMAIL",
  Sms = "SMS",
  Push = "PUSH",
}

// TODO: déclarer NotificationCreator (créateur abstrait) avec sa méthode de
// fabrication abstraite, puis un créateur concret par canal.
export abstract class NotificationCreator {
  protected abstract createNotification(): Notification;

  send(message: string): string {
    return this.createNotification().send(message);
  }
}

// TODO: remplacer ce stub par les créateurs concrets et leur table de
// correspondance, puis implémenter envoyerVia sans switch sur le type concret.
export function envoyerVia(_canal: NotificationChannel, _message: string): string {
  throw new Error("TODO: non implémenté");
}
