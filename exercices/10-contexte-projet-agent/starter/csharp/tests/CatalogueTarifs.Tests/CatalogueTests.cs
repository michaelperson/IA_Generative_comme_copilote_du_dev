using CatalogueTarifs;
using Xunit;

namespace CatalogueTarifs.Tests;

public class CatalogueTests
{
    private static readonly Catalogue Catalogue = new(new[]
    {
        new Tarif("CLA-100", "Clavier mécanique", 12990),
        new Tarif("ECR-240", "Écran 24 pouces", 19999),
        new Tarif("SOU-010", "Souris filaire", 999),
    });

    [Fact]
    public void TrouverParSku_RetourneLeTarif()
    {
        Assert.Equal(12990, Catalogue.TrouverParSku("CLA-100").PrixCentimes);
    }

    [Fact]
    public void TrouverParSku_LeveUneErreurTypee()
    {
        Assert.Throws<TarifIntrouvableException>(() => Catalogue.TrouverParSku("INEXISTANT"));
    }
}
