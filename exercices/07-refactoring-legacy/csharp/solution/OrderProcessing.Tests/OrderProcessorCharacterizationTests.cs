using Xunit;

namespace OrderProcessing.Tests;

/// <summary>
/// Tests de caractérisation (étape 2 de l'exercice) : ils figent le comportement
/// observé du OrderProcessor AVANT refactoring, pour au moins les 3 scénarios demandés
/// dans le prompt étape 2. Ils s'exécutent ici contre le OrderProcessor refactoré
/// (solution) sans avoir été modifiés (étape 4) : la signature publique et les valeurs
/// de retour sont restées identiques au comportement legacy d'origine.
/// </summary>
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
