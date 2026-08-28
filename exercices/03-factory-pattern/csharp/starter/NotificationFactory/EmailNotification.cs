namespace NotificationFactory;

/// <summary>
/// Notification envoyée par email.
/// </summary>
public class EmailNotification : INotification
{
    public Task<string> SendAsync(string message)
    {
        // TODO: simuler l'envoi d'un email et retourner une chaine de confirmation,
        // par exemple : $"[Email] envoyé : {message}"
        throw new NotImplementedException();
    }
}
