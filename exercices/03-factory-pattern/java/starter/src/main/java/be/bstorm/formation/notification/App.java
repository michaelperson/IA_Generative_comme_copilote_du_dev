package be.bstorm.formation.notification;

/**
 * Démo en ligne de commande : à compléter une fois les créateurs concrets écrits.
 */
public class App {

    public static void main(String[] args) {
        // TODO: une fois les créateurs concrets (EmailNotificationCreator, SmsNotificationCreator,
        // PushNotificationCreator) écrits, construire une table de correspondance
        // NotificationChannel -> NotificationCreator (ex. Map<NotificationChannel, NotificationCreator>)
        // et l'utiliser pour envoyer un message sur le canal préféré de l'utilisateur, par exemple :
        //
        // Map<NotificationChannel, NotificationCreator> creators = new EnumMap<>(NotificationChannel.class);
        // creators.put(NotificationChannel.EMAIL, new EmailNotificationCreator());
        // creators.put(NotificationChannel.SMS, new SmsNotificationCreator());
        // creators.put(NotificationChannel.PUSH, new PushNotificationCreator());
        //
        // NotificationChannel preferredChannel = NotificationChannel.PUSH;
        // String confirmation = creators.get(preferredChannel).send("Votre commande a été expédiée.");
        // System.out.println(confirmation);

        System.out.println("TODO: démo non implémentée.");
    }
}
