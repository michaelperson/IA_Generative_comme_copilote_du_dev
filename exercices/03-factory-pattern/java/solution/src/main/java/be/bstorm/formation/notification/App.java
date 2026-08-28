package be.bstorm.formation.notification;

import java.util.EnumMap;
import java.util.Map;

/**
 * Démo en ligne de commande montrant comment sélectionner un créateur concret
 * (Factory Method) à partir de la préférence utilisateur (NotificationChannel).
 */
public class App {

    /**
     * Table de correspondance entre le canal préféré de l'utilisateur et le
     * créateur concret associé. Ce point d'entrée est un simple aiguillage
     * pratique ; la fabrication proprement dite reste déléguée au Factory
     * Method de chaque {@link NotificationCreator}.
     */
    private static Map<NotificationChannel, NotificationCreator> buildCreators() {
        Map<NotificationChannel, NotificationCreator> creators = new EnumMap<>(NotificationChannel.class);
        creators.put(NotificationChannel.EMAIL, new EmailNotificationCreator());
        creators.put(NotificationChannel.SMS, new SmsNotificationCreator());
        creators.put(NotificationChannel.PUSH, new PushNotificationCreator());
        return creators;
    }

    public static void main(String[] args) {
        Map<NotificationChannel, NotificationCreator> creators = buildCreators();

        NotificationChannel preferredChannel = NotificationChannel.PUSH;

        NotificationCreator creator = creators.get(preferredChannel);
        String confirmation = creator.send("Votre commande a été expédiée.");

        System.out.println(confirmation);
    }
}
