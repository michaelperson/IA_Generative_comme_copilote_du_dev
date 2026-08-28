package be.bstorm.formation.notification;

/**
 * Notification envoyée par SMS.
 */
public class SmsNotification implements Notification {

    /**
     * Simule l'envoi d'un SMS (aucun appel réseau réel).
     */
    @Override
    public String send(String message) {
        return "[SMS] envoyé : " + message;
    }
}
