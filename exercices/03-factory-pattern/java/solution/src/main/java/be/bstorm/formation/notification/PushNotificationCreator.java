package be.bstorm.formation.notification;

/**
 * Créateur concret spécialisé dans la fabrication de {@link PushNotification}.
 */
public class PushNotificationCreator extends NotificationCreator {

    @Override
    protected Notification createNotification() {
        return new PushNotification();
    }
}
