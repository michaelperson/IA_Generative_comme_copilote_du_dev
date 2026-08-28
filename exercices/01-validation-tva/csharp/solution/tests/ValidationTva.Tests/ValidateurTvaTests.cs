using ValidationTva;
using Xunit;

namespace ValidationTva.Tests;

// Numéros de TVA valides utilisés dans ces tests :
//
//   1) numéro d'entreprise = 12345678
//      12345678 mod 97 = 3            (97 * 127275 = 12345675, reste 3)
//      clé attendue = 97 - 3 = 94
//      => BE1234567894
//
//   2) numéro d'entreprise = 87654321
//      87654321 mod 97 = 77           (97 * 903652 = 87654244, reste 77)
//      clé attendue = 97 - 77 = 20
//      => BE8765432120
//
//   3) cas limite (modulo 97 == 0) : numéro d'entreprise = 12338400
//      12338400 mod 97 = 0            (97 * 127200 = 12338400 exactement)
//      => la clé "attendue" vaudrait 97 - 0 = 97, ce qui n'est pas un cas valide :
//         le numéro doit être rejeté quelle que soit la clé fournie (ici "97").
public class ValidateurTvaTests
{
    [Fact]
    public void PremierNumeroValide_DoitEtreAccepte()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("BE1234567894");

        Assert.True(resultat.EstValide);
        Assert.Null(resultat.MessageErreur);
    }

    [Fact]
    public void SecondNumeroValide_DoitEtreAccepte()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("BE8765432120");

        Assert.True(resultat.EstValide);
        Assert.Null(resultat.MessageErreur);
    }

    [Fact]
    public void NumeroAvecSeparateurs_DoitEtreAccepteApresNettoyage()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("BE 1234.567.894");

        Assert.True(resultat.EstValide);
        Assert.Null(resultat.MessageErreur);
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

    [Fact]
    public void FormatSansPrefixeBe_DoitEtreRejete()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("1234567894");

        Assert.False(resultat.EstValide);
        Assert.NotNull(resultat.MessageErreur);
    }

    [Fact]
    public void CleDeControleFausseeDeUn_DoitEtreRejetee()
    {
        // Même numéro d'entreprise que le premier cas valide (12345678, clé attendue 94),
        // mais avec une clé fournie de 93 (fausse de 1).
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("BE1234567893");

        Assert.False(resultat.EstValide);
        Assert.NotNull(resultat.MessageErreur);
    }

    [Fact]
    public void NumeroEntrepriseMultipleDe97_DoitEtreRejeteCasLimite()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("BE1233840097");

        Assert.False(resultat.EstValide);
        Assert.NotNull(resultat.MessageErreur);
    }
}
