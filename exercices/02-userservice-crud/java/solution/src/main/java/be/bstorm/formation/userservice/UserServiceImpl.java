package be.bstorm.formation.userservice;

import com.fasterxml.jackson.databind.ObjectMapper;

import java.io.UncheckedIOException;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.net.http.HttpTimeoutException;
import java.time.Duration;
import java.util.List;
import java.util.Objects;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.CompletionException;

/**
 * Implémentation de référence de {@link UserService} consommant l'API REST
 * externe /users via {@link HttpClient} (java.net.http, injecté par le
 * constructeur — équivalent Java de IHttpClientFactory). Ne contient aucune
 * logique métier : uniquement l'accès aux données distantes.
 */
public class UserServiceImpl implements UserService {

    private static final Duration DEFAULT_REQUEST_TIMEOUT = Duration.ofSeconds(10);

    private final HttpClient httpClient;
    private final URI baseUri;
    private final Duration requestTimeout;
    private final ObjectMapper objectMapper = new ObjectMapper();

    public UserServiceImpl(HttpClient httpClient, URI baseUri) {
        this(httpClient, baseUri, DEFAULT_REQUEST_TIMEOUT);
    }

    /** Constructeur permettant de surcharger le timeout par défaut (10s), utile en tests. */
    public UserServiceImpl(HttpClient httpClient, URI baseUri, Duration requestTimeout) {
        this.httpClient = Objects.requireNonNull(httpClient, "httpClient");
        this.baseUri = Objects.requireNonNull(baseUri, "baseUri");
        this.requestTimeout = Objects.requireNonNull(requestTimeout, "requestTimeout");
    }

    @Override
    public CompletableFuture<User> getByIdAsync(String id) {
        return send(HttpRequest.newBuilder(baseUri.resolve("users/" + id)).GET(), User.class);
    }

    @Override
    public CompletableFuture<List<User>> getAllAsync() {
        return send(HttpRequest.newBuilder(baseUri.resolve("users")).GET(), List.class)
                .thenApply(list -> objectMapper.convertValue(list, objectMapper.getTypeFactory()
                        .constructCollectionType(List.class, User.class)));
    }

    @Override
    public CompletableFuture<User> createAsync(User user) {
        return send(HttpRequest.newBuilder(baseUri.resolve("users"))
                .POST(jsonBody(user)), User.class);
    }

    @Override
    public CompletableFuture<User> updateAsync(String id, User user) {
        return send(HttpRequest.newBuilder(baseUri.resolve("users/" + id))
                .PUT(jsonBody(user)), User.class);
    }

    @Override
    public CompletableFuture<Void> deleteAsync(String id) {
        return send(HttpRequest.newBuilder(baseUri.resolve("users/" + id)).DELETE(), Void.class)
                .thenApply(v -> null);
    }

    private HttpRequest.BodyPublisher jsonBody(User user) {
        try {
            return HttpRequest.BodyPublishers.ofString(objectMapper.writeValueAsString(user));
        } catch (Exception e) {
            throw new UncheckedIOException("Impossible de sérialiser le User en JSON", new java.io.IOException(e));
        }
    }

    /**
     * Envoie la requête HTTP construite, applique le timeout explicite de
     * {@link #requestTimeout}, lève {@link UserServiceException} pour tout code
     * non-2xx (avec statut et corps), {@link UserServiceTimeoutException} en cas
     * de dépassement du délai, et désérialise le corps JSON vers {@code responseType}.
     */
    private <T> CompletableFuture<T> send(HttpRequest.Builder requestBuilder, Class<T> responseType) {
        HttpRequest request = requestBuilder
                .timeout(requestTimeout)
                .header("Accept", "application/json")
                .build();

        return httpClient.sendAsync(request, HttpResponse.BodyHandlers.ofString())
                .thenApply(response -> {
                    int status = response.statusCode();
                    if (status < 200 || status >= 300) {
                        throw new UserServiceException(status, response.body());
                    }
                    String body = response.body();
                    if (responseType == Void.class || body == null || body.isBlank()) {
                        return null;
                    }
                    try {
                        return objectMapper.readValue(body, responseType);
                    } catch (Exception e) {
                        throw new UncheckedIOException("Réponse JSON invalide de l'API /users", new java.io.IOException(e));
                    }
                })
                .exceptionally(throwable -> {
                    Throwable cause = throwable instanceof CompletionException ? throwable.getCause() : throwable;
                    if (cause instanceof HttpTimeoutException) {
                        throw new UserServiceTimeoutException(
                                "La requête " + request.method() + " " + request.uri()
                                        + " vers l'API /users a dépassé le délai de "
                                        + requestTimeout.getSeconds() + " secondes.",
                                cause);
                    }
                    if (cause instanceof RuntimeException re) {
                        throw re;
                    }
                    throw new CompletionException(cause);
                });
    }
}
