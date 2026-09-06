namespace CatalogueTarifs;

/// <summary>Levée lorsque le pourcentage de remise sort de l'intervalle autorisé.</summary>
public class PourcentageRemiseInvalideException : Exception
{
    /// <summary>Pourcentage refusé.</summary>
    public int Pourcentage { get; }

    public PourcentageRemiseInvalideException(int pourcentage)
        : base($"Le pourcentage de remise doit être un entier de 0 à 100, reçu {pourcentage}.")
    {
        Pourcentage = pourcentage;
    }
}
