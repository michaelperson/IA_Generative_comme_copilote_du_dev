namespace NotificationFactory;

/// <summary>
/// Créateur abstrait du pattern Factory Method.
/// Chaque sous-classe concrète sait fabriquer UN type de notification
/// via la méthode de fabrication <see cref="CreateNotification"/>.
/// Le comportement métier commun (ici l'envoi) est défini une seule fois
/// dans cette classe et délègue la création du produit aux sous-classes :
/// c'est ce découplage entre "quoi faire" (Send) et "quoi créer"
/// (CreateNotification) qui distingue le Factory Method d'une Simple Factory.
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

/// <summary>
/// Créateur concret spécialisé dans la fabrication d'<see cref="EmailNotification"/>.
/// </summary>
public class EmailNotificationCreator : NotificationCreator
{
    protected override INotification CreateNotification() => new EmailNotification();
}

/// <summary>
/// Créateur concret spécialisé dans la fabrication de <see cref="SmsNotification"/>.
/// </summary>
public class SmsNotificationCreator : NotificationCreator
{
    protected override INotification CreateNotification() => new SmsNotification();
}

/// <summary>
/// Créateur concret spécialisé dans la fabrication de <see cref="PushNotification"/>.
/// </summary>
public class PushNotificationCreator : NotificationCreator
{
    protected override INotification CreateNotification() => new PushNotification();
}
