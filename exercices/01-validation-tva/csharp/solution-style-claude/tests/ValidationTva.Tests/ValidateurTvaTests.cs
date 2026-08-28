using ValidationTva;
using Xunit;

namespace ValidationTva.Tests;

/// <summary>
/// Suite de tests pour <see cref="ValidateurTva"/>.
/// </summary>
/// <remarks>
/// Numéros de TVA valides utilisés dans ces tests, avec le détail du calcul :
/// <list type="bullet">
/// <item>
/// BE1234567894 : numéro d'entreprise 12345678, 12345678 mod 97 = 3
/// (car 97 * 127275 = 12345675, reste 3), clé attendue = 97 - 3 = 94.
/// </item>
/// <item>
/// BE8765432120 : numéro d'entreprise 87654321, 87654321 mod 97 = 77
/// (car 97 * 903652 = 87654244, reste 77), clé attendue = 97 - 77 = 20.
/// </item>
/// <item>
/// BE1233840097 : numéro d'entreprise 12338400, 12338400 mod 97 = 0
/// (car 97 * 127200 = 12338400 exactement) : cas limite, le numéro doit être
/// rejeté quelle que soit la clé fournie ("97" ici).
/// </item>
/// </list>
/// </remarks>
public class ValidateurTvaTests
{
    [Fact]
    public void ValiderNumeroTvaBelge_AvecPremierNumeroValide_RetourneEstValideTrue()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("BE1234567894");

        Assert.True(resultat.EstValide);
        Assert.Null(resultat.MessageErreur);
    }

    [Fact]
    public void ValiderNumeroTvaBelge_AvecSecondNumeroValide_RetourneEstValideTrue()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("BE8765432120");

        Assert.True(resultat.EstValide);
        Assert.Null(resultat.MessageErreur);
    }

    [Fact]
    public void ValiderNumeroTvaBelge_AvecSeparateursEspacesEtPoints_EstAccepteApresNettoyage()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("BE 1234.567.894");

        Assert.True(resultat.EstValide);
        Assert.Null(resultat.MessageErreur);
    }

    [Fact]
    public void ValiderNumeroTvaBelge_AvecEntreeVide_RetourneResultatInvalideSansException()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("");

        Assert.False(resultat.EstValide);
        Assert.NotNull(resultat.MessageErreur);
    }

    [Fact]
    public void ValiderNumeroTvaBelge_AvecEntreeNull_RetourneResultatInvalideSansException()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge(null);

        Assert.False(resultat.EstValide);
        Assert.NotNull(resultat.MessageErreur);
    }

    [Fact]
    public void ValiderNumeroTvaBelge_SansPrefixeBe_RetourneFormatInvalide()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("1234567894");

        Assert.False(resultat.EstValide);
        Assert.NotNull(resultat.MessageErreur);
    }

    [Fact]
    public void ValiderNumeroTvaBelge_AvecCleDeControleFausseeDeUn_RetourneCleInvalide()
    {
        // Même numéro d'entreprise que le premier cas valide (clé attendue 94),
        // mais avec une clé fournie de 93.
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("BE1234567893");

        Assert.False(resultat.EstValide);
        Assert.NotNull(resultat.MessageErreur);
    }

    [Fact]
    public void ValiderNumeroTvaBelge_AvecNumeroEntrepriseMultipleDe97_RetourneCasLimiteInvalide()
    {
        var resultat = ValidateurTva.ValiderNumeroTvaBelge("BE1233840097");

        Assert.False(resultat.EstValide);
        Assert.NotNull(resultat.MessageErreur);
    }
}
