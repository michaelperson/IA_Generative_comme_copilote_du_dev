package be.bstorm.formation.userservice;

import java.net.URI;
import java.net.http.HttpClient;
import java.util.List;
import java.util.Objects;
import java.util.concurrent.CompletableFuture;

/**
 * Implémentation de {@link UserService} consommant l'API REST externe /users.
 * EXERCICE : complétez les méthodes ci-dessous. Consultez le README.md de l'exercice
 * pour le prompt fourni et la liste des contraintes à respecter (client HTTP injecté,
 * exception typée sur non-2xx, sérialisation JSON, timeout de 10s).
 */
public class UserServiceImpl implements UserService {

    private final HttpClient httpClient;
    private final URI baseUri;

    public UserServiceImpl(HttpClient httpClient, URI baseUri) {
        this.httpClient = Objects.requireNonNull(httpClient, "httpClient");
        this.baseUri = Objects.requireNonNull(baseUri, "baseUri");
    }

    @Override
    public CompletableFuture<User> getByIdAsync(String id) {
        // TODO: GET {baseUri}/users/{id} via httpClient.sendAsync(...).
        // - désérialiser le JSON de réponse vers User
        // - lever UserServiceException si le code HTTP n'est pas 2xx (statut + corps)
        // - respecter un timeout de 10 secondes sur la requête -> UserServiceTimeoutException
        throw new UnsupportedOperationException("TODO: getByIdAsync n'est pas encore implémenté");
    }

    @Override
    public CompletableFuture<List<User>> getAllAsync() {
        // TODO: GET {baseUri}/users, mêmes contraintes que getByIdAsync.
        throw new UnsupportedOperationException("TODO: getAllAsync n'est pas encore implémenté");
    }

    @Override
    public CompletableFuture<User> createAsync(User user) {
        // TODO: POST {baseUri}/users avec le corps JSON sérialisé, mêmes contraintes.
        throw new UnsupportedOperationException("TODO: createAsync n'est pas encore implémenté");
    }

    @Override
    public CompletableFuture<User> updateAsync(String id, User user) {
        // TODO: PUT {baseUri}/users/{id} avec le corps JSON sérialisé, mêmes contraintes.
        throw new UnsupportedOperationException("TODO: updateAsync n'est pas encore implémenté");
    }

    @Override
    public CompletableFuture<Void> deleteAsync(String id) {
        // TODO: DELETE {baseUri}/users/{id}, mêmes contraintes (pas de corps de réponse attendu).
        throw new UnsupportedOperationException("TODO: deleteAsync n'est pas encore implémenté");
    }
}
