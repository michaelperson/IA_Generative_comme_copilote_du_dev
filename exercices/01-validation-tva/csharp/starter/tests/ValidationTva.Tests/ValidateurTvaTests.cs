using ValidationTva;
using Xunit;

namespace ValidationTva.Tests;

// Calcul du numéro de TVA valide utilisé ci-dessous :
//   numéro d'entreprise = 12345678
//   12345678 mod 97 = 3            (12345678 = 97 * 127275 + 3)
//   clé de contrôle attendue = 97 - 3 = 94
//   => numéro complet valide : BE1234567894
public class ValidateurTvaTests
{
    [Fact]
    public void NumeroValide_DoitEtreAccepte()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("BE1234567894");

        Assert.True(resultat.EstValide);
        Assert.Null(resultat.MessageErreur);
    }

    [Fact]
    public void CleDeControleFausse_DoitEtreRejetee()
    {
        // Même numéro d'entreprise (12345678) mais clé de contrôle 93 au lieu de 94.
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("BE1234567893");

        Assert.False(resultat.EstValide);
        Assert.NotNull(resultat.MessageErreur);
    }

    [Fact]
    public void FormatIncorrect_DoitEtreRejete()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("FR1234567894");

        Assert.False(resultat.EstValide);
        Assert.NotNull(resultat.MessageErreur);
    }

    [Fact]
    public void EntreeVide_DoitEtreRejeteeSansException()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("");

        Assert.False(resultat.EstValide);
        Assert.NotNull(resultat.MessageErreur);
    }

    [Fact]
    public void EntreeNull_DoitEtreRejeteeSansException()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge(null);

        Assert.False(resultat.EstValide);
        Assert.NotNull(resultat.MessageErreur);
    }
}
