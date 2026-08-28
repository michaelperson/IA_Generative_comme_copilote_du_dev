namespace NotificationFactory;

/// <summary>
/// Démo en ligne de commande : à compléter une fois les créateurs concrets écrits.
/// </summary>
public static class Program
{
    public static async Task Main(string[] args)
    {
        // TODO: une fois les créateurs concrets (EmailNotificationCreator, SmsNotificationCreator,
        // PushNotificationCreator) écrits, construire une table de correspondance
        // NotificationChannel -> NotificationCreator (ex. Dictionary<NotificationChannel, NotificationCreator>)
        // et l'utiliser pour envoyer un message sur le canal préféré de l'utilisateur, par exemple :
        //
        // var creators = new Dictionary<NotificationChannel, NotificationCreator>
        // {
        //     [NotificationChannel.Email] = new EmailNotificationCreator(),
        //     [NotificationChannel.Sms] = new SmsNotificationCreator(),
        //     [NotificationChannel.Push] = new PushNotificationCreator(),
        // };
        //
        // var preferredChannel = NotificationChannel.Push;
        // string confirmation = await creators[preferredChannel].Send("Votre commande a été expédiée.");
        // Console.WriteLine(confirmation);

        await Task.CompletedTask;
        Console.WriteLine("TODO: démo non implémentée.");
    }
}
