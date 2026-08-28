using Xunit;

namespace OrderProcessing.Tests;

public class OrderProcessorCharacterizationTests
{
    [Fact]
    public void ClientVipPremiereCommandeBelgique_EstValideeAutomatiquement()
    {
        var processor = new OrderProcessor();

        string result = processor.ProcessOrder("VIP", 1000, "BE", true);

        Assert.Equal("Commande de 847 EUR - validee automatiquement", result);
    }

    [Fact]
    public void ClientRegulierRecurrentFrance_EstValideeAutomatiquement()
    {
        var processor = new OrderProcessor();

        string result = processor.ProcessOrder("REGULAR", 500, "FR", false);

        Assert.Equal("Commande de 570 EUR - validee automatiquement", result);
    }

    [Fact]
    public void MontantDepassantLeSeuil_NecessiteValidationManager()
    {
        var processor = new OrderProcessor();

        string result = processor.ProcessOrder("REGULAR", 2000, "DE", false);

        Assert.Equal("Commande de 2261 EUR - necessite validation manager", result);
    }
}
