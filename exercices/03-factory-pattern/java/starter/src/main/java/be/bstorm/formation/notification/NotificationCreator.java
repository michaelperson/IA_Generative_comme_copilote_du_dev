package be.bstorm.formation.notification;

/**
 * Créateur abstrait du pattern Factory Method.
 * Chaque sous-classe concrète sait fabriquer UN type de notification via la
 * méthode de fabrication {@link #createNotification()}.
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

// TODO: créer les créateurs concrets qui héritent de NotificationCreator,
// un par canal, en surchargeant createNotification() :
//   - EmailNotificationCreator : retourne un EmailNotification
//   - SmsNotificationCreator   : retourne un SmsNotification
//   - PushNotificationCreator  : retourne un PushNotification
//
// Attention : le but est une véritable application du Factory Method
// (une méthode de fabrication abstraite surchargée par sous-classe),
// PAS un simple switch/if dans une méthode statique unique.
