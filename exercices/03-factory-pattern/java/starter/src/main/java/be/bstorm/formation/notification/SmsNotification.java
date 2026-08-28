package be.bstorm.formation.notification;

/**
 * Notification envoyée par SMS.
 */
public class SmsNotification implements Notification {

    @Override
    public String send(String message) {
        // TODO: simuler l'envoi d'un SMS et retourner une chaine de confirmation,
        // par exemple : "[SMS] envoyé : " + message
        throw new UnsupportedOperationException("TODO: implémenter SmsNotification.send");
    }
}
