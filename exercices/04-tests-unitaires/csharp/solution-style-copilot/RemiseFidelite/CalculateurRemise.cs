namespace RemiseFidelite;

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
        taux = Math.Min(taux, 0.25m); // plafond à 25%

        return Math.Round(montantCommande * taux, 2, MidpointRounding.AwayFromZero);
    }
}
