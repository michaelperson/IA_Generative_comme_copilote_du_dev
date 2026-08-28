package be.bstorm.formation.notification;

/**
 * Notification envoyée par push (application mobile/desktop).
 */
public class PushNotification implements Notification {

    @Override
    public String send(String message) {
        // TODO: simuler l'envoi d'une notification push et retourner une chaine de confirmation,
        // par exemple : "[Push] envoyé : " + message
        throw new UnsupportedOperationException("TODO: implémenter PushNotification.send");
    }
}
