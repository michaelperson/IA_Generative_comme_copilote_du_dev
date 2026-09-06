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

    [Fact]
    public void AppliquerRemise_RetourneUnNouveauTarif()
    {
        var remise = Catalogue.AppliquerRemise("CLA-100", 10);

        Assert.Equal(11691, remise.PrixCentimes);
        Assert.Equal("CLA-100", remise.Sku);
        Assert.Equal("Clavier mécanique", remise.Libelle);
    }

    [Fact]
    public void AppliquerRemise_LaisseLeTarifDorigineInchange()
    {
        Catalogue.AppliquerRemise("ECR-240", 25);

        Assert.Equal(19999, Catalogue.TrouverParSku("ECR-240").PrixCentimes);
    }

    [Theory]
    [InlineData(0, 999)]
    [InlineData(100, 0)]
    public void AppliquerRemise_AccepteLesBornes(int pourcentage, long attendu)
    {
        Assert.Equal(attendu, Catalogue.AppliquerRemise("SOU-010", pourcentage).PrixCentimes);
    }

    [Theory]
    // 999 x 95 = 94905 ; /100 = 949,05 -> 949
    [InlineData("SOU-010", 5, 949)]
    // 19999 x 75 = 1499925 ; /100 = 14999,25 -> 14999
    [InlineData("ECR-240", 25, 14999)]
    // 999 x 50 = 49950 ; /100 = 499,50 -> 500 (loin de zéro, pas 499)
    [InlineData("SOU-010", 50, 500)]
    public void AppliquerRemise_ArronditAuCentimeLoinDeZero(string sku, int pourcentage, long attendu)
    {
        Assert.Equal(attendu, Catalogue.AppliquerRemise(sku, pourcentage).PrixCentimes);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void AppliquerRemise_RefuseUnPourcentageInvalide(int pourcentage)
    {
        Assert.Throws<PourcentageRemiseInvalideException>(
            () => Catalogue.AppliquerRemise("CLA-100", pourcentage));
    }

    [Fact]
    public void AppliquerRemise_PropageLerreurDeSkuInconnu()
    {
        Assert.Throws<TarifIntrouvableException>(() => Catalogue.AppliquerRemise("INEXISTANT", 10));
    }
}
