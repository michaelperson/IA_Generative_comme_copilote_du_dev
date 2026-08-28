using System.Runtime.ExceptionServices;

namespace RetryPolicy;

/// <summary>
/// BONUS — hors périmètre strict de l'exercice (qui porte sur la documentation, pas la
/// correction). Variante corrigée de <see cref="RetryPolicy"/> qui montre à quoi ressemble
/// le correctif du défaut signalé dans <c>INCOHERENCES.md</c> : au lieu de lever une
/// <see cref="InvalidOperationException"/> générique après épuisement des tentatives, on capture
/// la dernière exception rencontrée et on la repropage telle quelle (type, message et stack
/// trace d'origine préservés) grâce à <see cref="ExceptionDispatchInfo"/>.
/// </summary>
public class RetryPolicyFixed
{
    private readonly int _maxAttempts;
    private readonly TimeSpan _baseDelay;

    public RetryPolicyFixed(int maxAttempts, TimeSpan baseDelay)
    {
        _maxAttempts = maxAttempts;
        _baseDelay = baseDelay;
    }

    /// <summary>
    /// Identique à <see cref="RetryPolicy.ExecuteAsync{T}"/>, à ceci près qu'en cas d'échec de
    /// toutes les tentatives, c'est la DERNIÈRE exception réellement rencontrée qui est
    /// repropagée (via <see cref="ExceptionDispatchInfo.Throw"/>), et non plus une
    /// <see cref="InvalidOperationException"/> générique masquant la cause originale.
    /// </summary>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        ExceptionDispatchInfo? lastException = null;

        for (int attempt = 1; attempt <= _maxAttempts; attempt++)
        {
            try
            {
                return await action();
            }
            catch (Exception ex) when (attempt < _maxAttempts)
            {
                lastException = ExceptionDispatchInfo.Capture(ex);
                var delay = TimeSpan.FromMilliseconds(_baseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                await Task.Delay(delay);
            }
        }

        // Toutes les tentatives ont échoué : on repropage la dernière exception rencontrée
        // plutôt que d'en masquer la cause derrière une exception générique.
        lastException?.Throw();
        throw new InvalidOperationException("Unreachable"); // ne devrait jamais être atteint si maxAttempts >= 1
    }
}
