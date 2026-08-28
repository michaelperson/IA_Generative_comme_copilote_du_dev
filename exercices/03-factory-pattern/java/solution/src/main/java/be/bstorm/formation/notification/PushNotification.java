package be.bstorm.formation.notification;

/**
 * Notification envoyée par push (application mobile/desktop).
 */
public class PushNotification implements Notification {

    /**
     * Simule l'envoi d'une notification push (aucun appel réseau réel).
     */
    @Override
    public String send(String message) {
        return "[Push] envoyé : " + message;
    }
}
