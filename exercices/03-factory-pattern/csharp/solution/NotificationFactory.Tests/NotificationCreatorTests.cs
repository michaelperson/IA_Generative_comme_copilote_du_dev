using Xunit;

namespace NotificationFactory.Tests;

public class NotificationCreatorTests
{
    [Fact]
    public void EmailNotificationCreator_CreatesEmailNotification()
    {
        var creator = new TestableEmailCreator();

        INotification notification = creator.ExposeCreateNotification();

        Assert.IsType<EmailNotification>(notification);
    }

    [Fact]
    public void SmsNotificationCreator_CreatesSmsNotification()
    {
        var creator = new TestableSmsCreator();

        INotification notification = creator.ExposeCreateNotification();

        Assert.IsType<SmsNotification>(notification);
    }

    [Fact]
    public void PushNotificationCreator_CreatesPushNotification()
    {
        var creator = new TestablePushCreator();

        INotification notification = creator.ExposeCreateNotification();

        Assert.IsType<PushNotification>(notification);
    }

    [Fact]
    public async Task EmailNotificationCreator_Send_ReturnsEmailConfirmation()
    {
        var creator = new EmailNotificationCreator();

        string result = await creator.Send("Bonjour");

        Assert.Contains("Email", result);
        Assert.Contains("Bonjour", result);
    }

    [Fact]
    public async Task SmsNotificationCreator_Send_ReturnsSmsConfirmation()
    {
        var creator = new SmsNotificationCreator();

        string result = await creator.Send("Bonjour");

        Assert.Contains("SMS", result);
        Assert.Contains("Bonjour", result);
    }

    [Fact]
    public async Task PushNotificationCreator_Send_ReturnsPushConfirmation()
    {
        var creator = new PushNotificationCreator();

        string result = await creator.Send("Bonjour");

        Assert.Contains("Push", result);
        Assert.Contains("Bonjour", result);
    }

    // Ces sous-classes de test exposent publiquement la méthode de fabrication
    // "protected" héritée de NotificationCreator : un membre protégé reste
    // accessible depuis une classe dérivée, ce qui permet de vérifier
    // indépendamment de Send() que chaque créateur concret fabrique bien le
    // bon type de produit (comportement caractéristique du Factory Method,
    // à distinguer d'un simple switch).
    private sealed class TestableEmailCreator : EmailNotificationCreator
    {
        public INotification ExposeCreateNotification() => CreateNotification();
    }

    private sealed class TestableSmsCreator : SmsNotificationCreator
    {
        public INotification ExposeCreateNotification() => CreateNotification();
    }

    private sealed class TestablePushCreator : PushNotificationCreator
    {
        public INotification ExposeCreateNotification() => CreateNotification();
    }
}
