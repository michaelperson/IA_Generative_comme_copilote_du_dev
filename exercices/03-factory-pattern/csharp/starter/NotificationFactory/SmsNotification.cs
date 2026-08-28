namespace NotificationFactory;

/// <summary>
/// Notification envoyée par SMS.
/// </summary>
public class SmsNotification : INotification
{
    public Task<string> SendAsync(string message)
    {
        // TODO: simuler l'envoi d'un SMS et retourner une chaine de confirmation,
        // par exemple : $"[SMS] envoyé : {message}"
        throw new NotImplementedException();
    }
}
