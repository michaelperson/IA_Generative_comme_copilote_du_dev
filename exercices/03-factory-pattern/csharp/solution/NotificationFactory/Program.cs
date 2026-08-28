namespace NotificationFactory;

/// <summary>
/// Démo en ligne de commande montrant comment sélectionner un créateur concret
/// (Factory Method) à partir de la préférence utilisateur (NotificationChannel).
/// </summary>
public static class Program
{
    /// <summary>
    /// Table de correspondance entre le canal préféré de l'utilisateur et le
    /// créateur concret associé. Ce point d'entrée est un simple aiguillage
    /// pratique ; la fabrication proprement dite reste déléguée au Factory
    /// Method de chaque <see cref="NotificationCreator"/>.
    /// </summary>
    private static readonly Dictionary<NotificationChannel, NotificationCreator> Creators = new()
    {
        [NotificationChannel.Email] = new EmailNotificationCreator(),
        [NotificationChannel.Sms] = new SmsNotificationCreator(),
        [NotificationChannel.Push] = new PushNotificationCreator(),
    };

    public static async Task Main(string[] args)
    {
        NotificationChannel preferredChannel = NotificationChannel.Push;

        NotificationCreator creator = Creators[preferredChannel];
        string confirmation = await creator.Send("Votre commande a été expédiée.");

        Console.WriteLine(confirmation);
    }
}
