package be.bstorm.formation.notification;

/**
 * Créateur concret spécialisé dans la fabrication de {@link SmsNotification}.
 */
public class SmsNotificationCreator extends NotificationCreator {

    @Override
    protected Notification createNotification() {
        return new SmsNotification();
    }
}
