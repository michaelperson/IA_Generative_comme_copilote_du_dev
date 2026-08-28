namespace RemiseFidelite;

// Fonction métier fournie pour l'exercice 4 (tests unitaires) : elle est déjà
// complète et correcte. Il n'y a rien à implémenter ici — l'objectif de
// l'exercice est d'écrire (avec l'aide de l'IA) la suite de tests unitaires
// dans RemiseFidelite.Tests/CalculateurRemiseTests.cs.
public static class CalculateurRemise
{
    public static decimal CalculerMontantRemise(decimal montantCommande, int anneesFidelite, bool estClientPremium)
    {
        if (montantCommande < 0)
            throw new ArgumentOutOfRangeException(nameof(montantCommande));

        decimal tauxBase = anneesFidelite switch
        {
            >= 10 => 0.15m,
            >= 5  => 0.10m,
            >= 1  => 0.05m,
            _     => 0m
        };

        decimal taux = estClientPremium ? tauxBase + 0.05m : tauxBase;
        taux = Math.Min(taux, 0.25m); // plafond contractuel à 25%

        return Math.Round(montantCommande * taux, 2, MidpointRounding.AwayFromZero);
    }
}
