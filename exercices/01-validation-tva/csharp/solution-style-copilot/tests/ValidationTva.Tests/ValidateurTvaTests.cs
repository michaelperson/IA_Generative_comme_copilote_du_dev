using ValidationTva;
using Xunit;

namespace ValidationTva.Tests;

// BE1234567894 : 12345678 mod 97 = 3, clé = 97 - 3 = 94
// BE8765432120 : 87654321 mod 97 = 77, clé = 97 - 77 = 20
// BE1233840097 : 12338400 mod 97 = 0 -> cas limite, invalide
public class ValidateurTvaTests
{
    [Fact]
    public void NumeroValide()
    {
        var r = ValidateurTva.ValiderNumeroTvaBelge("BE1234567894");
        Assert.True(r.EstValide);
    }

    [Fact]
    public void NumeroValideAvecSeparateurs()
    {
        var r = ValidateurTva.ValiderNumeroTvaBelge("BE 8765.432.120");
        Assert.True(r.EstValide);
    }

    [Fact]
    public void CleInvalide()
    {
        var r = ValidateurTva.ValiderNumeroTvaBelge("BE1234567893");
        Assert.False(r.EstValide);
    }

    [Fact]
    public void FormatInvalide()
    {
        var r = ValidateurTva.ValiderNumeroTvaBelge("FR1234567894");
        Assert.False(r.EstValide);
    }

    [Fact]
    public void EntreeVide()
    {
        var r = ValidateurTva.ValiderNumeroTvaBelge("");
        Assert.False(r.EstValide);
    }

    [Fact]
    public void EntreeNull()
    {
        var r = ValidateurTva.ValiderNumeroTvaBelge(null);
        Assert.False(r.EstValide);
    }

    [Fact]
    public void CasLimiteModulo97Zero()
    {
        var r = ValidateurTva.ValiderNumeroTvaBelge("BE1233840097");
        Assert.False(r.EstValide);
    }
}
