namespace NotificationFactory;

/// <summary>
/// Notification envoyée par push (application mobile/desktop).
/// </summary>
public class PushNotification : INotification
{
    /// <summary>
    /// Simule l'envoi d'une notification push (aucun appel réseau réel).
    /// </summary>
    public Task<string> SendAsync(string message)
    {
        string confirmation = $"[Push] envoyé : {message}";
        return Task.FromResult(confirmation);
    }
}
