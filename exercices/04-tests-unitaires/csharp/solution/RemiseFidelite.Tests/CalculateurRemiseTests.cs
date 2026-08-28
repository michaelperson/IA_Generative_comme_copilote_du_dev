using RemiseFidelite;
using Xunit;

namespace RemiseFidelite.Tests;

/// <summary>
/// Suite de tests de référence pour <see cref="CalculateurRemise.CalculerMontantRemise"/>.
/// Couvre les cas nominaux par palier d'ancienneté, le bonus client premium,
/// le plafond contractuel à 25%, et les cas limites/erreurs identifiés lors de
/// l'analyse d'un premier jet de tests trop générique (montant = 0,
/// anneesFidelite = 0, montant négatif, arrondi).
/// </summary>
public class CalculateurRemiseTests
{
    // Cas nominaux : un par palier d'ancienneté (0, 1-4, 5-9, >=10 ans), sans puis
    // avec le bonus premium. Les valeurs attendues sont passées en double dans
    // l'attribut (les attributs .NET n'acceptent pas de littéraux decimal) puis
    // reconverties en decimal dans le corps du test.
    [Theory]
    [InlineData(1000, 0, false, 0.00)]   // anneesFidelite = 0 -> aucune remise (cas limite explicitement demandé)
    [InlineData(1000, 3, false, 50.00)]  // palier 1-4 ans -> 5%
    [InlineData(1000, 7, false, 100.00)] // palier 5-9 ans -> 10%
    [InlineData(1000, 10, false, 150.00)] // palier >= 10 ans -> 15%
    [InlineData(1000, 25, false, 150.00)] // très ancien client -> reste sur le même palier (pas de palier au-delà de 10 ans)
    [InlineData(1000, 0, true, 50.00)]   // anneesFidelite = 0 + premium -> seul le bonus premium s'applique (5%)
    [InlineData(1000, 3, true, 100.00)]  // 5% + 5% premium
    [InlineData(1000, 7, true, 150.00)]  // 10% + 5% premium
    [InlineData(1000, 10, true, 200.00)] // 15% + 5% premium = 20% (taux réel maximal, cf. test dédié au plafond)
    public void CalculerMontantRemise_CasNominauxEtPremium_RetourneLeMontantAttendu(
        double montantCommande, int anneesFidelite, bool estClientPremium, double montantRemiseAttendu)
    {
        var resultat = CalculateurRemise.CalculerMontantRemise((decimal)montantCommande, anneesFidelite, estClientPremium);

        Assert.Equal((decimal)montantRemiseAttendu, resultat);
    }

    [Fact]
    public void CalculerMontantRemise_MontantCommandeZero_RetourneZero()
    {
        var resultat = CalculateurRemise.CalculerMontantRemise(0m, anneesFidelite: 10, estClientPremium: true);

        Assert.Equal(0m, resultat);
    }

    [Fact]
    public void CalculerMontantRemise_MontantCommandeNegatif_LeveArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            CalculateurRemise.CalculerMontantRemise(-0.01m, anneesFidelite: 5, estClientPremium: false));
    }

    [Fact]
    public void CalculerMontantRemise_ArrondiSurUneDemieUnite_UtiliseLArrondiLoinDeZero()
    {
        // 1.3 * 5% = 0.065 : exactement à mi-chemin entre 0.06 et 0.07.
        // MidpointRounding.AwayFromZero doit arrondir vers 0.07 (et non 0.06,
        // comme le ferait un arrondi bancaire "ToEven"). Un premier jet de tests
        // généré sans préciser ce mode de calcul manque souvent cette nuance.
        var resultat = CalculateurRemise.CalculerMontantRemise(1.3m, anneesFidelite: 3, estClientPremium: false);

        Assert.Equal(0.07m, resultat);
    }

    [Fact]
    public void CalculerMontantRemise_TauxMaximalReel_NeDeclenchePasLePlafondDe25Pourcent()
    {
        // Point de vigilance identifié lors de l'analyse du premier jet (étape 2/3
        // de l'exercice) : avec les paliers actuels (>= 10 ans => 15%, + 5% premium),
        // le taux réellement atteignable culmine à 20%. Le plafond contractuel de
        // 25% codé dans la méthode ne peut donc JAMAIS s'activer avec les entrées
        // publiques actuelles de la fonction. Plutôt que d'affirmer un cas à 25%
        // qui n'existe pas dans le domaine réel de la fonction (ce qui serait un
        // test mathématiquement faux), ce test documente ce constat : c'est un
        // point à remonter au métier (le plafond est-il "mort", ou prévu pour des
        // paliers futurs plus généreux ?).
        var resultat = CalculateurRemise.CalculerMontantRemise(1000m, anneesFidelite: 10, estClientPremium: true);

        Assert.Equal(200m, resultat);
    }
}
