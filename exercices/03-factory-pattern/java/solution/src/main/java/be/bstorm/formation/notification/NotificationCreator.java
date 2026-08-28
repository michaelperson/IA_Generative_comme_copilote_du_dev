package be.bstorm.formation.notification;

/**
 * Créateur abstrait du pattern Factory Method.
 * Chaque sous-classe concrète sait fabriquer UN type de notification via la
 * méthode de fabrication {@link #createNotification()}. Le comportement
 * métier commun (ici l'envoi) est défini une seule fois dans cette classe et
 * délègue la création du produit aux sous-classes : c'est ce découplage
 * entre "quoi faire" (send) et "quoi créer" (createNotification) qui
 * distingue le Factory Method d'une Simple Factory.
 */
public abstract class NotificationCreator {

    /**
     * Méthode de fabrication (factory method) : chaque sous-classe concrète
     * retourne l'implémentation de {@link Notification} qui lui correspond.
     */
    protected abstract Notification createNotification();

    /**
     * Logique commune qui s'appuie sur le produit fabriqué par la sous-classe,
     * sans jamais connaître le type concret de notification instancié.
     */
    public String send(String message) {
        Notification notification = createNotification();
        return notification.send(message);
    }
}
