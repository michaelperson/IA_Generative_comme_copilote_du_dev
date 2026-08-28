using System.Text;
using System.Text.Json;

namespace UserServiceCrud;

/// <summary>
/// Implémentation de référence de <see cref="IUserService"/> qui consomme l'API REST
/// externe /users via un <see cref="HttpClient"/> obtenu depuis <see cref="IHttpClientFactory"/>.
/// Ne contient aucune logique métier : uniquement l'accès aux données distantes.
/// </summary>
public class UserService : IUserService
{
    /// <summary>Nom du HttpClient nommé à enregistrer côté DI (services.AddHttpClient("UsersApi", ...)).</summary>
    public const string HttpClientName = "UsersApi";

    private static readonly TimeSpan DefaultRequestTimeout = TimeSpan.FromSeconds(10);

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        // Sérialisation JSON attendue par l'API : camelCase sur le fil, PascalCase côté modèle C#.
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TimeSpan _requestTimeout;

    /// <summary>
    /// Crée une nouvelle instance de <see cref="UserService"/>.
    /// </summary>
    /// <param name="httpClientFactory">Fabrique fournissant le HttpClient nommé "UsersApi".</param>
    /// <param name="requestTimeout">
    /// Délai maximal accordé à chaque requête HTTP avant de lever un <see cref="TimeoutException"/>.
    /// Vaut 10 secondes par défaut (paramétrable pour faciliter les tests unitaires).
    /// </param>
    public UserService(IHttpClientFactory httpClientFactory, TimeSpan? requestTimeout = null)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _requestTimeout = requestTimeout ?? DefaultRequestTimeout;
    }

    /// <inheritdoc />
    public Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => SendAsync<User>(HttpMethod.Get, $"users/{id}", body: null, cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await SendAsync<List<User>>(HttpMethod.Get, "users", body: null, cancellationToken)
            .ConfigureAwait(false);
        return users;
    }

    /// <inheritdoc />
    public Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
        => SendAsync<User>(HttpMethod.Post, "users", user, cancellationToken);

    /// <inheritdoc />
    public Task<User> UpdateAsync(int id, User user, CancellationToken cancellationToken = default)
        => SendAsync<User>(HttpMethod.Put, $"users/{id}", user, cancellationToken);

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await SendAsync<object?>(HttpMethod.Delete, $"users/{id}", body: null, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Envoie une requête HTTP vers l'API /users : gère le timeout explicite, la levée de
    /// <see cref="UserServiceException"/> pour tout code non-2xx, et la désérialisation JSON
    /// (camelCase côté API) vers le type <typeparamref name="TResponse"/> (PascalCase côté modèle).
    /// </summary>
    private async Task<TResponse> SendAsync<TResponse>(
        HttpMethod method,
        string relativeUrl,
        object? body,
        CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(HttpClientName);

        using var request = new HttpRequestMessage(method, relativeUrl);
        if (body is not null)
        {
            var payload = JsonSerializer.Serialize(body, JsonOptions);
            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
        }

        // Timeout explicite de 10s (par défaut) combiné au CancellationToken de l'appelant :
        // si c'est le délai interne (et non le token appelant) qui déclenche l'annulation,
        // on la traduit en TimeoutException explicite plutôt que de laisser fuiter une
        // OperationCanceledException générique.
        using var timeoutCts = new CancellationTokenSource(_requestTimeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

        HttpResponseMessage response;
        try
        {
            response = await httpClient.SendAsync(request, linkedCts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new TimeoutException(
                $"La requête {method} {relativeUrl} vers l'API /users a dépassé le délai de {_requestTimeout.TotalSeconds} secondes.");
        }

        using (response)
        {
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new UserServiceException((int)response.StatusCode, responseBody);
            }

            // DELETE ne renvoie généralement pas de corps exploitable : TResponse vaut alors object?.
            if (typeof(TResponse) == typeof(object) || string.IsNullOrWhiteSpace(responseBody))
            {
                return default!;
            }

            var result = JsonSerializer.Deserialize<TResponse>(responseBody, JsonOptions);
            return result!;
        }
    }
}
