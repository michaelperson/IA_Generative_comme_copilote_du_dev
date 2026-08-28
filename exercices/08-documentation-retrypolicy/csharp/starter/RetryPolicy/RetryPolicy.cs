namespace RetryPolicy;

// TODO: cette classe n'a aucune documentation. C'est l'objet de l'exercice :
// utilisez le prompt fourni dans le README pour générer une documentation
// professionnelle (commentaires XML <summary>, <param>, <typeparam>, <returns>,
// <exception>) qui explique le POURQUOI (backoff exponentiel) et pas seulement
// le QUOI, et qui signale toute incohérence détectée dans le code ci-dessous.
//
// Ne modifiez pas la logique de cette classe : l'exercice porte sur la
// documentation d'un code existant, pas sur sa correction.
public class RetryPolicy
{
    private readonly int _maxAttempts;
    private readonly TimeSpan _baseDelay;

    public RetryPolicy(int maxAttempts, TimeSpan baseDelay)
    {
        _maxAttempts = maxAttempts;
        _baseDelay = baseDelay;
    }

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
