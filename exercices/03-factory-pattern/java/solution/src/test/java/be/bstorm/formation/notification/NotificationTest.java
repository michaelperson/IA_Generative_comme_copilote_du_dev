package be.bstorm.formation.notification;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertTrue;

class NotificationTest {

    @Test
    void emailNotificationSendReturnsConfirmation() {
        Notification notification = new EmailNotification();

        String result = notification.send("hello");

        assertTrue(result.contains("Email"));
        assertTrue(result.contains("hello"));
    }

    @Test
    void smsNotificationSendReturnsConfirmation() {
        Notification notification = new SmsNotification();

        String result = notification.send("hello");

        assertTrue(result.contains("SMS"));
        assertTrue(result.contains("hello"));
    }

    @Test
    void pushNotificationSendReturnsConfirmation() {
        Notification notification = new PushNotification();

        String result = notification.send("hello");

        assertTrue(result.contains("Push"));
        assertTrue(result.contains("hello"));
    }
}
