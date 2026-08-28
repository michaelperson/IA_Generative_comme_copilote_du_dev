package be.bstorm.formation.notification;

/**
 * Notification envoyée par email.
 */
public class EmailNotification implements Notification {

    @Override
    public String send(String message) {
        // TODO: simuler l'envoi d'un email et retourner une chaine de confirmation,
        // par exemple : "[Email] envoyé : " + message
        throw new UnsupportedOperationException("TODO: implémenter EmailNotification.send");
    }
}
