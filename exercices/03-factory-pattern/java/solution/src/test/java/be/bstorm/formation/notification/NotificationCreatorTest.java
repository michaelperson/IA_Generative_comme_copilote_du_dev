package be.bstorm.formation.notification;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertInstanceOf;
import static org.junit.jupiter.api.Assertions.assertTrue;

class NotificationCreatorTest {

    // Ces sous-classes de test exposent publiquement la méthode de fabrication
    // "protected" héritée de NotificationCreator : un membre protégé reste
    // accessible depuis une classe dérivée, ce qui permet de vérifier
    // indépendamment de send() que chaque créateur concret fabrique bien le
    // bon type de produit (comportement caractéristique du Factory Method,
    // à distinguer d'un simple switch).
    private static class TestableEmailCreator extends EmailNotificationCreator {
        Notification exposeCreateNotification() {
            return createNotification();
        }
    }

    private static class TestableSmsCreator extends SmsNotificationCreator {
        Notification exposeCreateNotification() {
            return createNotification();
        }
    }

    private static class TestablePushCreator extends PushNotificationCreator {
        Notification exposeCreateNotification() {
            return createNotification();
        }
    }

    @Test
    void emailNotificationCreatorCreatesEmailNotification() {
        TestableEmailCreator creator = new TestableEmailCreator();

        Notification notification = creator.exposeCreateNotification();

        assertInstanceOf(EmailNotification.class, notification);
    }

    @Test
    void smsNotificationCreatorCreatesSmsNotification() {
        TestableSmsCreator creator = new TestableSmsCreator();

        Notification notification = creator.exposeCreateNotification();

        assertInstanceOf(SmsNotification.class, notification);
    }

    @Test
    void pushNotificationCreatorCreatesPushNotification() {
        TestablePushCreator creator = new TestablePushCreator();

        Notification notification = creator.exposeCreateNotification();

        assertInstanceOf(PushNotification.class, notification);
    }

    @Test
    void emailNotificationCreatorSendReturnsEmailConfirmation() {
        NotificationCreator creator = new EmailNotificationCreator();

        String result = creator.send("Bonjour");

        assertTrue(result.contains("Email"));
        assertTrue(result.contains("Bonjour"));
    }

    @Test
    void smsNotificationCreatorSendReturnsSmsConfirmation() {
        NotificationCreator creator = new SmsNotificationCreator();

        String result = creator.send("Bonjour");

        assertTrue(result.contains("SMS"));
        assertTrue(result.contains("Bonjour"));
    }

    @Test
    void pushNotificationCreatorSendReturnsPushConfirmation() {
        NotificationCreator creator = new PushNotificationCreator();

        String result = creator.send("Bonjour");

        assertTrue(result.contains("Push"));
        assertTrue(result.contains("Bonjour"));
    }
}
