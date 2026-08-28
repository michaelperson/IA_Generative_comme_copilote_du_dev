namespace NotificationFactory;

/// <summary>
/// Notification envoyée par email.
/// </summary>
public class EmailNotification : INotification
{
    /// <summary>
    /// Simule l'envoi d'un email (aucun appel réseau réel).
    /// </summary>
    public Task<string> SendAsync(string message)
    {
        string confirmation = $"[Email] envoyé : {message}";
        return Task.FromResult(confirmation);
    }
}
