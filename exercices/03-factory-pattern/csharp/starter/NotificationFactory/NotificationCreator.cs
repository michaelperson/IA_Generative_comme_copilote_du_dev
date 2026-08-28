namespace NotificationFactory;

/// <summary>
/// Créateur abstrait du pattern Factory Method.
/// Chaque sous-classe concrète sait fabriquer UN type de notification
/// via la méthode de fabrication <see cref="CreateNotification"/>.
/// Le comportement métier commun (ici l'envoi) est défini une seule fois
/// dans cette classe et délègue la création du produit aux sous-classes.
/// </summary>
public abstract class NotificationCreator
{
    /// <summary>
    /// Méthode de fabrication (factory method) : chaque sous-classe concrète
    /// retourne l'implémentation d'<see cref="INotification"/> qui lui correspond.
    /// </summary>
    protected abstract INotification CreateNotification();

    /// <summary>
    /// Logique commune qui s'appuie sur le produit fabriqué par la sous-classe,
    /// sans jamais connaître le type concret de notification instancié.
    /// </summary>
    public Task<string> Send(string message)
    {
        INotification notification = CreateNotification();
        return notification.SendAsync(message);
    }
}

// TODO: créer les créateurs concrets qui héritent de NotificationCreator,
// un par canal, en surchargeant CreateNotification() :
//   - EmailNotificationCreator : retourne un EmailNotification
//   - SmsNotificationCreator   : retourne un SmsNotification
//   - PushNotificationCreator  : retourne un PushNotification
//
// Attention : le but est une véritable application du Factory Method
// (une méthode de fabrication abstraite surchargée par sous-classe),
// PAS un simple switch/if dans une méthode statique unique.
