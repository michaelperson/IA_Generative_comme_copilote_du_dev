package be.bstorm.formation.notification;

/**
 * Créateur concret spécialisé dans la fabrication d'{@link EmailNotification}.
 */
public class EmailNotificationCreator extends NotificationCreator {

    @Override
    protected Notification createNotification() {
        return new EmailNotification();
    }
}
