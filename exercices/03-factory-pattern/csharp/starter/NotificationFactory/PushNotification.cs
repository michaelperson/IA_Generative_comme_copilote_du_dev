namespace NotificationFactory;

/// <summary>
/// Notification envoyée par push (application mobile/desktop).
/// </summary>
public class PushNotification : INotification
{
    public Task<string> SendAsync(string message)
    {
        // TODO: simuler l'envoi d'une notification push et retourner une chaine de confirmation,
        // par exemple : $"[Push] envoyé : {message}"
        throw new NotImplementedException();
    }
}
