using RemiseFidelite;
using Xunit;

namespace RemiseFidelite.Tests;

/// <summary>
/// Suite de tests pour <see cref="CalculateurRemise.CalculerMontantRemise"/>.
/// </summary>
/// <remarks>
/// Organisation :
/// <list type="bullet">
/// <item>Cas nominaux, palier par palier, y compris les frontières de chaque palier.</item>
/// <item>Bonus premium, cumulé avec chaque palier.</item>
/// <item>Plafond contractuel à 25% et étude de son atteignabilité réelle.</item>
/// <item>Cas limites explicitement demandés dans la consigne : montant = 0,
/// anneesFidelite = 0, montant négatif.</item>
/// <item>Comportement de l'arrondi (mode "loin de zéro").</item>
/// </list>
/// </remarks>
public class CalculateurRemiseTests
{
    // --- Cas nominaux : un par palier d'ancienneté ---------------------------------

    [Fact]
    public void Given_ClientSansAnciennete_When_CalculDeLaRemise_Then_AucuneRemiseAppliquee()
    {
        // anneesFidelite = 0 : cas limite explicitement demandé dans la consigne.
        var resultat = CalculateurRemise.CalculerMontantRemise(1000m, anneesFidelite: 0, estClientPremium: false);

        Assert.Equal(0m, resultat);
    }

    [Fact]
    public void Given_ClientDansLePalierUnAQuatreAns_When_CalculDeLaRemise_Then_TauxDeCinqPourCentApplique()
    {
        var resultat = CalculateurRemise.CalculerMontantRemise(1000m, anneesFidelite: 3, estClientPremium: false);

        Assert.Equal(50m, resultat);
    }

    [Fact]
    public void Given_ClientDansLePalierCinqANeufAns_When_CalculDeLaRemise_Then_TauxDeDixPourCentApplique()
    {
        var resultat = CalculateurRemise.CalculerMontantRemise(1000m, anneesFidelite: 7, estClientPremium: false);

        Assert.Equal(100m, resultat);
    }

    [Fact]
    public void Given_ClientDansLePalierDixAnsOuPlus_When_CalculDeLaRemise_Then_TauxDeQuinzePourCentApplique()
    {
        var resultat = CalculateurRemise.CalculerMontantRemise(1000m, anneesFidelite: 10, estClientPremium: false);

        Assert.Equal(150m, resultat);
    }

    [Theory]
    [InlineData(0, 0.00)]   // frontière basse du palier "0" : 0 an
    [InlineData(1, 50.00)]  // frontière basse du palier "1-4 ans"
    [InlineData(4, 50.00)]  // frontière haute du palier "1-4 ans"
    [InlineData(5, 100.00)] // frontière basse du palier "5-9 ans"
    [InlineData(9, 100.00)] // frontière haute du palier "5-9 ans"
    [InlineData(10, 150.00)] // frontière basse du palier "10 ans et plus"
    [InlineData(40, 150.00)] // très loin dans le palier "10 ans et plus" : le taux ne bouge plus
    public void Given_AnneesFideliteAUneFrontiereDePalier_When_CalculDeLaRemise_Then_LeBonPalierEstApplique(
        int anneesFidelite, double montantRemiseAttendu)
    {
        // Ces frontières sont le point exact où un premier jet de tests généré trop
        // vite ("teste la fonction") a tendance à ne tester qu'une valeur "typique"
        // par palier, sans vérifier les bornes (off-by-one sur les seuils 1, 5, 10).
        var resultat = CalculateurRemise.CalculerMontantRemise(1000m, anneesFidelite, estClientPremium: false);

        Assert.Equal((decimal)montantRemiseAttendu, resultat);
    }

    // --- Bonus client premium, cumulé avec chaque palier ---------------------------

    [Theory]
    [InlineData(0, 50.00)]   // 0% de base + 5% premium
    [InlineData(3, 100.00)]  // 5% de base + 5% premium
    [InlineData(7, 150.00)]  // 10% de base + 5% premium
    [InlineData(10, 200.00)] // 15% de base + 5% premium = 20% (taux réel maximal, voir test dédié au plafond)
    public void Given_ClientPremium_When_CalculDeLaRemise_Then_UnBonusDeCinqPointsEstAjouteAuTauxDeBase(
        int anneesFidelite, double montantRemiseAttendu)
    {
        var resultat = CalculateurRemise.CalculerMontantRemise(1000m, anneesFidelite, estClientPremium: true);

        Assert.Equal((decimal)montantRemiseAttendu, resultat);
    }

    // --- Plafond contractuel à 25% ---------------------------------------------------

    [Fact]
    public void Given_MeilleurCombinaisonPossible_When_CalculDeLaRemise_Then_LeTauxReelPlafonneANaturelement20PourCent()
    {
        // Point de vigilance identifié lors de l'itération sur le prompt (étapes 2 et 3
        // de l'exercice) : "le plafond à 25% est-il testé à sa frontière exacte ?"
        //
        // Réponse, après analyse : avec les paliers d'ancienneté actuels (maximum 15%
        // à partir de 10 ans) et le bonus premium (+5%), le taux le plus élevé qu'il
        // est possible d'obtenir via l'API publique de la fonction est 15% + 5% = 20%.
        // Le plafond de 25% codé dans Math.Min(taux, 0.25m) ne peut donc jamais
        // s'activer avec les entrées actuelles : c'est du code défensif pour une
        // évolution future des paliers (ou un signal à remonter au métier si ce n'est
        // pas intentionnel). Écrire un test qui affirmerait un résultat plafonné à 25%
        // serait mathématiquement faux pour cette version de la fonction ; ce test
        // documente donc explicitement le taux réel maximal (20%) plutôt qu'un
        // scénario inatteignable.
        var resultat = CalculateurRemise.CalculerMontantRemise(1000m, anneesFidelite: 10, estClientPremium: true);

        Assert.Equal(200m, resultat);
    }

    // --- Cas limites explicitement demandés dans la consigne ------------------------

    [Fact]
    public void Given_MontantCommandeNul_When_CalculDeLaRemise_Then_LaRemiseEstNulle()
    {
        // montantCommande = 0 : cas limite explicitement demandé dans la consigne.
        // Quel que soit le taux appliqué, une base à 0 doit toujours donner 0,
        // sans division par zéro ni autre effet de bord.
        var resultat = CalculateurRemise.CalculerMontantRemise(0m, anneesFidelite: 10, estClientPremium: true);

        Assert.Equal(0m, resultat);
    }

    [Fact]
    public void Given_MontantCommandeNegatif_When_CalculDeLaRemise_Then_UneArgumentOutOfRangeExceptionEstLevee()
    {
        // montantCommande négatif : cas limite/erreur explicitement demandé dans la
        // consigne. On vérifie à la fois le type d'exception et, via nameof, que le
        // message identifie bien le bon paramètre pour faciliter le diagnostic.
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            CalculateurRemise.CalculerMontantRemise(-0.01m, anneesFidelite: 5, estClientPremium: false));

        Assert.Equal("montantCommande", exception.ParamName);
    }

    // --- Comportement de l'arrondi ---------------------------------------------------

    [Fact]
    public void Given_UnMontantExactementAMiCheminEntreDeuxCentimes_When_CalculDeLaRemise_Then_LArrondiSeFaitLoinDeZero()
    {
        // 1.3 * 5% = 0.065, exactement à mi-chemin entre 0.06 et 0.07. La fonction
        // utilise MidpointRounding.AwayFromZero, qui doit arrondir vers 0.07.
        // Un arrondi bancaire ("ToEven", le défaut implicite de beaucoup de code
        // généré sans précision) donnerait 0.06 : ce test échouerait alors et
        // révélerait une régression sur le mode d'arrondi, ce qu'un premier jet
        // de tests générique omet fréquemment de vérifier.
        var resultat = CalculateurRemise.CalculerMontantRemise(1.3m, anneesFidelite: 3, estClientPremium: false);

        Assert.Equal(0.07m, resultat);
    }
}
