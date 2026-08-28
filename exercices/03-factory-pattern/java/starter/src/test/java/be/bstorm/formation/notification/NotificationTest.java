package be.bstorm.formation.notification;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertTrue;

class NotificationTest {

    @Test
    void emailNotificationSendReturnsConfirmation() {
        Notification notification = new EmailNotification();

        // TODO (starter) : ce test échoue tant que EmailNotification.send
        // n'est pas implémenté (il lève UnsupportedOperationException).
        String result = notification.send("hello");

        assertTrue(result.contains("Email"));
    }

    @Test
    void smsNotificationSendReturnsConfirmation() {
        Notification notification = new SmsNotification();

        // TODO (starter) : ce test échoue tant que SmsNotification.send
        // n'est pas implémenté (il lève UnsupportedOperationException).
        String result = notification.send("hello");

        assertTrue(result.contains("SMS"));
    }

    @Test
    void pushNotificationSendReturnsConfirmation() {
        Notification notification = new PushNotification();

        // TODO (starter) : ce test échoue tant que PushNotification.send
        // n'est pas implémenté (il lève UnsupportedOperationException).
        String result = notification.send("hello");

        assertTrue(result.contains("Push"));
    }

    // TODO: une fois les créateurs concrets écrits (EmailNotificationCreator,
    // SmsNotificationCreator, PushNotificationCreator), ajouter des tests qui
    // vérifient que chaque créateur produit bien le type de notification attendu
    // (véritable Factory Method, pas un switch déguisé).
}
