/**
 * Exercice 3 — Factory Method appliqué à une fabrique de notifications.
 *
 * Implémentation de référence : une véritable application du Factory Method
 * (une méthode de fabrication abstraite, surchargée par chaque créateur
 * concret), et non un switch déguisé en « factory ».
 */

/** Contrat commun à tous les canaux de notification. */
export interface Notification {
  /** Envoie le message et retourne une confirmation (envoi simulé). */
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

/** Canal choisi selon la préférence utilisateur. */
export enum NotificationChannel {
  Email = "EMAIL",
  Sms = "SMS",
  Push = "PUSH",
}

/**
 * Créateur abstrait du pattern Factory Method.
 * Chaque sous-classe concrète sait fabriquer UN type de notification.
 */
export abstract class NotificationCreator {
  /** Méthode de fabrication : surchargée par chaque créateur concret. */
  protected abstract createNotification(): Notification;

  /**
   * Logique commune, qui s'appuie sur le produit fabriqué par la sous-classe
   * sans jamais connaître le type concret instancié.
   */
  send(message: string): string {
    return this.createNotification().send(message);
  }
}

export class EmailNotificationCreator extends NotificationCreator {
  protected createNotification(): Notification {
    return new EmailNotification();
  }
}

export class SmsNotificationCreator extends NotificationCreator {
  protected createNotification(): Notification {
    return new SmsNotification();
  }
}

export class PushNotificationCreator extends NotificationCreator {
  protected createNotification(): Notification {
    return new PushNotification();
  }
}

/**
 * Table de correspondance canal -> créateur concret.
 *
 * Point d'entrée pratique volontairement EXTERNE au pattern : c'est ce qui
 * permet de choisir dynamiquement le créateur sans resimplifier le Factory
 * Method en Simple Factory.
 */
export const CREATEURS: Readonly<Record<NotificationChannel, NotificationCreator>> = {
  [NotificationChannel.Email]: new EmailNotificationCreator(),
  [NotificationChannel.Sms]: new SmsNotificationCreator(),
  [NotificationChannel.Push]: new PushNotificationCreator(),
};

export function envoyerVia(canal: NotificationChannel, message: string): string {
  return CREATEURS[canal].send(message);
}
