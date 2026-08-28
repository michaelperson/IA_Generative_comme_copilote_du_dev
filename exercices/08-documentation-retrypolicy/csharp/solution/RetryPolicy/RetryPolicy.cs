namespace RetryPolicy;

/// <summary>
/// Fiabilise l'exécution d'un appel réseau vers un service externe instable en le
/// ré-essayant automatiquement selon une stratégie de backoff exponentiel.
/// </summary>
/// <remarks>
/// Le backoff exponentiel (le délai entre deux tentatives double à chaque nouvel échec :
/// 1x, 2x, 4x, 8x... la valeur de <c>baseDelay</c>) existe pour une raison précise : lorsqu'un
/// service externe est déjà en difficulté (surcharge, redémarrage, dégradation temporaire),
/// ré-essayer immédiatement et à intervalle fixe ne fait qu'ajouter de la pression sur un
/// système qui a justement besoin de temps pour se rétablir. Espacer les tentatives de façon
/// croissante laisse au service la marge nécessaire pour se stabiliser avant le prochain appel,
/// tout en évitant de multiplier les tentatives synchronisées de nombreux clients au même
/// instant (effet de "troupeau").
///
/// <para>
/// INCOHÉRENCE DÉTECTÉE (voir aussi <c>INCOHERENCES.md</c>) : lorsque toutes les tentatives
/// ont échoué, <see cref="ExecuteAsync{T}"/> ne propage PAS la dernière exception réellement
/// rencontrée. Elle lève à la place une <see cref="InvalidOperationException"/> générique avec
/// le message "Unreachable", ce qui masque complètement la cause originale de l'échec
/// (message, type d'exception, stack trace du service appelé). C'est un défaut réel du code,
/// pas un choix de conception volontaire : un appelant qui catch cette exception ne peut ni la
/// distinguer d'un vrai bug interne, ni savoir pourquoi l'appel a échoué.
/// </para>
/// </remarks>
public class RetryPolicy
{
    private readonly int _maxAttempts;
    private readonly TimeSpan _baseDelay;

    /// <summary>
    /// Crée une politique de retry avec backoff exponentiel.
    /// </summary>
    /// <param name="maxAttempts">
    /// Nombre maximum de tentatives (la tentative initiale comprise). Par exemple,
    /// <c>maxAttempts = 3</c> signifie un appel initial et jusqu'à deux ré-essais.
    /// </param>
    /// <param name="baseDelay">
    /// Délai de base utilisé pour calculer le backoff exponentiel. Le délai réel avant la
    /// tentative <c>n</c> vaut <c>baseDelay * 2^(n-1)</c>.
    /// </param>
    public RetryPolicy(int maxAttempts, TimeSpan baseDelay)
    {
        _maxAttempts = maxAttempts;
        _baseDelay = baseDelay;
    }

    /// <summary>
    /// Exécute <paramref name="action"/> en la ré-essayant automatiquement, avec un délai
    /// croissant exponentiellement entre chaque tentative, tant que le nombre maximum de
    /// tentatives n'est pas atteint.
    /// </summary>
    /// <remarks>
    /// Le backoff exponentiel entre deux tentatives protège le service externe appelé : plutôt
    /// que de marteler un service déjà instable à intervalle fixe, on lui laisse de plus en
    /// plus de temps pour se rétablir avant chaque nouvelle tentative.
    /// </remarks>
    /// <typeparam name="T">Type du résultat retourné par <paramref name="action"/>.</typeparam>
    /// <param name="action">L'opération asynchrone à exécuter (typiquement un appel réseau).</param>
    /// <returns>Le résultat de <paramref name="action"/> dès qu'une tentative réussit.</returns>
    /// <exception cref="InvalidOperationException">
    /// Levée après épuisement de toutes les tentatives (<c>maxAttempts</c> échecs).
    /// ATTENTION — comportement actuel à corriger : cette exception est TOUJOURS générique
    /// ("Unreachable") et NE PORTE PAS la dernière exception réellement rencontrée pendant les
    /// tentatives. La cause originale de l'échec (le vrai type d'exception, son message, sa
    /// stack trace) est silencieusement perdue. Voir <c>RetryPolicyFixed</c> pour un correctif
    /// qui propage la dernière exception capturée via <c>ExceptionDispatchInfo</c>.
    /// </exception>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        for (int attempt = 1; attempt <= _maxAttempts; attempt++)
        {
            try
            {
                return await action();
            }
            catch (Exception) when (attempt < _maxAttempts)
            {
                var delay = TimeSpan.FromMilliseconds(_baseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                await Task.Delay(delay);
            }
        }
        throw new InvalidOperationException("Unreachable");
    }
}
