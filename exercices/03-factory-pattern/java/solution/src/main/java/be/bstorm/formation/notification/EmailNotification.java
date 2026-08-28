package be.bstorm.formation.notification;

/**
 * Notification envoyée par email.
 */
public class EmailNotification implements Notification {

    /**
     * Simule l'envoi d'un email (aucun appel réseau réel).
     */
    @Override
    public String send(String message) {
        return "[Email] envoyé : " + message;
    }
}
