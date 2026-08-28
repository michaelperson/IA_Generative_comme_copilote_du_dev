namespace UserServiceCrud;

/// <summary>
/// Contrat d'accès aux données pour la ressource /users de l'API REST externe.
/// Aucune logique métier ne doit se trouver dans une implémentation de cette interface :
/// uniquement l'accès aux données (lecture/écriture via HTTP).
/// </summary>
public interface IUserService
{
    /// <summary>Récupère un utilisateur par son identifiant (GET /users/{id}).</summary>
    /// <exception cref="UserServiceException">L'API a répondu avec un code HTTP non-2xx.</exception>
    /// <exception cref="TimeoutException">La requête a dépassé le délai de 10 secondes.</exception>
    Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Récupère la liste complète des utilisateurs (GET /users).</summary>
    /// <exception cref="UserServiceException">L'API a répondu avec un code HTTP non-2xx.</exception>
    /// <exception cref="TimeoutException">La requête a dépassé le délai de 10 secondes.</exception>
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Crée un nouvel utilisateur (POST /users).</summary>
    /// <exception cref="UserServiceException">L'API a répondu avec un code HTTP non-2xx.</exception>
    /// <exception cref="TimeoutException">La requête a dépassé le délai de 10 secondes.</exception>
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>Met à jour un utilisateur existant (PUT /users/{id}).</summary>
    /// <exception cref="UserServiceException">L'API a répondu avec un code HTTP non-2xx.</exception>
    /// <exception cref="TimeoutException">La requête a dépassé le délai de 10 secondes.</exception>
    Task<User> UpdateAsync(int id, User user, CancellationToken cancellationToken = default);

    /// <summary>Supprime un utilisateur (DELETE /users/{id}).</summary>
    /// <exception cref="UserServiceException">L'API a répondu avec un code HTTP non-2xx.</exception>
    /// <exception cref="TimeoutException">La requête a dépassé le délai de 10 secondes.</exception>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
