import { describe, expect, it } from "vitest";
import {
  EmailNotification,
  NotificationChannel,
  NotificationCreator,
  PushNotification,
  SmsNotification,
  envoyerVia,
} from "../src/notification.js";

describe("canaux de notification", () => {
  it("chaque canal formate sa propre confirmation", () => {
    expect(new EmailNotification().send("test")).toBe("[Email] envoyé : test");
    expect(new SmsNotification().send("test")).toBe("[SMS] envoyé : test");
    expect(new PushNotification().send("test")).toBe("[Push] envoyé : test");
  });
});

describe("Factory Method", () => {
  it.each([
    [NotificationChannel.Email, "[Email] envoyé : Votre commande a été expédiée."],
    [NotificationChannel.Sms, "[SMS] envoyé : Votre commande a été expédiée."],
    [NotificationChannel.Push, "[Push] envoyé : Votre commande a été expédiée."],
  ])("envoie via %s", (canal, attendu) => {
    expect(envoyerVia(canal, "Votre commande a été expédiée.")).toBe(attendu);
  });

  it("un créateur personnalisé s'intègre sans modifier le code existant", () => {
    // Preuve que la structure est bien ouverte à l'extension : on ajoute un
    // canal sans toucher aux créateurs déjà écrits. Impossible avec un switch.
    class WebhookNotificationCreator extends NotificationCreator {
      protected createNotification() {
        return { send: (m: string) => `[Webhook] envoyé : ${m}` };
      }
    }
    expect(new WebhookNotificationCreator().send("ok")).toBe("[Webhook] envoyé : ok");
  });
});
