using Xunit;

namespace NotificationFactory.Tests;

public class NotificationTests
{
    [Fact]
    public async Task EmailNotification_Send_ReturnsConfirmation()
    {
        var notification = new EmailNotification();

        // TODO (starter) : ce test échoue tant que EmailNotification.SendAsync
        // n'est pas implémenté (il lève NotImplementedException).
        string result = await notification.SendAsync("hello");

        Assert.Contains("Email", result);
    }

    [Fact]
    public async Task SmsNotification_Send_ReturnsConfirmation()
    {
        var notification = new SmsNotification();

        // TODO (starter) : ce test échoue tant que SmsNotification.SendAsync
        // n'est pas implémenté (il lève NotImplementedException).
        string result = await notification.SendAsync("hello");

        Assert.Contains("SMS", result);
    }

    [Fact]
    public async Task PushNotification_Send_ReturnsConfirmation()
    {
        var notification = new PushNotification();

        // TODO (starter) : ce test échoue tant que PushNotification.SendAsync
        // n'est pas implémenté (il lève NotImplementedException).
        string result = await notification.SendAsync("hello");

        Assert.Contains("Push", result);
    }

    // TODO: une fois les créateurs concrets écrits (EmailNotificationCreator,
    // SmsNotificationCreator, PushNotificationCreator), ajouter des tests qui
    // vérifient que chaque créateur produit bien le type de notification attendu
    // (véritable Factory Method, pas un switch déguisé).
}
