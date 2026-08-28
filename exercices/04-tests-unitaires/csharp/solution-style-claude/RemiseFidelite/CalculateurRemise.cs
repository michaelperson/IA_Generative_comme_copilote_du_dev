namespace RemiseFidelite;

/// <summary>
/// Calcule le montant de la remise applicable à une commande, en fonction de
/// l'ancienneté du client (en années de fidélité) et de son éventuel statut
/// premium.
/// </summary>
/// <remarks>
/// Règles métier appliquées, par ordre :
/// <list type="number">
/// <item>Un taux de base est déterminé par palier d'ancienneté : 0% en dessous
/// d'un an, 5% de 1 à 4 ans, 10% de 5 à 9 ans, 15% à partir de 10 ans.</item>
/// <item>Un client premium bénéficie d'un bonus supplémentaire de 5 points.</item>
/// <item>Le taux total est plafonné contractuellement à 25%.</item>
/// <item>Le montant de la remise est arrondi à 2 décimales, en arrondissant
/// la demi-unité loin de zéro (0.005 -> 0.01, pas 0.00).</item>
/// </list>
/// </remarks>
public static class CalculateurRemise
{
    /// <summary>
    /// Calcule le montant de la remise (en euros) applicable à une commande.
    /// </summary>
    /// <param name="montantCommande">Montant total de la commande. Doit être positif ou nul.</param>
    /// <param name="anneesFidelite">Nombre d'années d'ancienneté du client.</param>
    /// <param name="estClientPremium">Indique si le client bénéficie du statut premium.</param>
    /// <returns>Le montant de la remise, arrondi à 2 décimales.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Levée si <paramref name="montantCommande"/> est négatif : un montant de commande
    /// ne peut pas être négatif dans le domaine métier de cette fonction.
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
        taux = Math.Min(taux, 0.25m); // plafond contractuel à 25%, quel que soit le cumul base + premium

        return Math.Round(montantCommande * taux, 2, MidpointRounding.AwayFromZero);
    }
}
