namespace NotificationFactory;

/// <summary>
/// Contrat commun à tous les canaux de notification.
/// </summary>
public interface INotification
{
    /// <summary>
    /// Envoie le message via le canal concret et retourne une confirmation
    /// décrivant l'envoi (simulation, aucun appel réseau réel n'est effectué).
    /// </summary>
    Task<string> SendAsync(string message);
}
