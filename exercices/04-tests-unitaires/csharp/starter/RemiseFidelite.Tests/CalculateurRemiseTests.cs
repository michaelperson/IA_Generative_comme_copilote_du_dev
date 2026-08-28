using RemiseFidelite;
using Xunit;

namespace RemiseFidelite.Tests;

public class CalculateurRemiseTests
{
    // TODO : la suite de tests complète est à construire dans le cadre de l'exercice 4.
    // Ce test minimal ne sert qu'à vérifier que le projet compile et s'exécute (vert).
    // Objectif : couvrir les cas nominaux (paliers d'ancienneté), le bonus premium,
    // le plafond contractuel à 25%, et les cas limites/erreurs (montant = 0,
    // anneesFidelite = 0, montant négatif).
    [Fact]
    public void CalculerMontantRemise_CasBasique_NeLevePasException()
    {
        var resultat = CalculateurRemise.CalculerMontantRemise(100m, 0, false);

        Assert.Equal(0m, resultat);
    }
}
