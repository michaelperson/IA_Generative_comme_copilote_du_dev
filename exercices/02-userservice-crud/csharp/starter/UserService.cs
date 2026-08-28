namespace UserServiceCrud;

/// <summary>
/// Implémentation de <see cref="IUserService"/> consommant l'API REST externe /users.
/// EXERCICE : complétez les méthodes ci-dessous. Consultez le README.md de l'exercice
/// pour le prompt fourni et la liste des contraintes à respecter (client HTTP injecté,
/// exception typée sur non-2xx, sérialisation JSON camelCase/PascalCase, timeout de 10s).
/// </summary>
public class UserService : IUserService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        // TODO: GET /users/{id} via un HttpClient créé par _httpClientFactory.
        // - désérialiser le JSON (camelCase) vers User (PascalCase)
        // - lever UserServiceException si le code HTTP n'est pas 2xx (statut + corps)
        // - respecter un timeout de 10 secondes -> TimeoutException explicite
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // TODO: GET /users, mêmes contraintes que GetByIdAsync.
        throw new NotImplementedException();
    }

    public Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        // TODO: POST /users avec le corps JSON sérialisé (camelCase), mêmes contraintes.
        throw new NotImplementedException();
    }

    public Task<User> UpdateAsync(int id, User user, CancellationToken cancellationToken = default)
    {
        // TODO: PUT /users/{id} avec le corps JSON sérialisé (camelCase), mêmes contraintes.
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        // TODO: DELETE /users/{id}, mêmes contraintes (pas de corps de réponse attendu).
        throw new NotImplementedException();
    }
}
