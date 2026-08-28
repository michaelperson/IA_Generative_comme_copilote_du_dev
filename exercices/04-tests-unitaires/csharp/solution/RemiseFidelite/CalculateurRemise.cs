namespace RemiseFidelite;

/// <summary>
/// Calcule le montant de la remise applicable à une commande, en fonction de
/// l'ancienneté du client et de son statut premium.
/// </summary>
/// <remarks>
/// Fonction métier fournie pour l'exercice 4 (tests unitaires) : elle est déjà
/// complète et correcte. L'objectif de l'exercice est d'écrire, avec l'aide de
/// l'IA, la suite de tests unitaires qui la couvre (cf. CalculateurRemiseTests.cs).
/// </remarks>
public static class CalculateurRemise
{
    /// <summary>
    /// Calcule le montant de la remise (en euros) applicable à une commande.
    /// </summary>
    /// <param name="montantCommande">Montant total de la commande. Doit être positif ou nul.</param>
    /// <param name="anneesFidelite">Nombre d'années d'ancienneté du client.</param>
    /// <param name="estClientPremium">Indique si le client bénéficie du statut premium.</param>
    /// <returns>Le montant de la remise, arrondi à 2 décimales (arrondi "loin de zéro").</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Levée si <paramref name="montantCommande"/> est négatif.
    /// </exception>
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
