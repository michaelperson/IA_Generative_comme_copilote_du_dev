namespace NotificationFactory;

/// <summary>
/// Notification envoyée par SMS.
/// </summary>
public class SmsNotification : INotification
{
    /// <summary>
    /// Simule l'envoi d'un SMS (aucun appel réseau réel).
    /// </summary>
    public Task<string> SendAsync(string message)
    {
        string confirmation = $"[SMS] envoyé : {message}";
        return Task.FromResult(confirmation);
    }
}
