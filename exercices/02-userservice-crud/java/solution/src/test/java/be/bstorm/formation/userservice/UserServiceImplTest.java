package be.bstorm.formation.userservice;

import com.sun.net.httpserver.HttpServer;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.URI;
import java.net.http.HttpClient;
import java.nio.charset.StandardCharsets;
import java.time.Duration;
import java.util.List;
import java.util.concurrent.CompletionException;
import java.util.concurrent.TimeUnit;

import static org.junit.jupiter.api.Assertions.*;

/**
 * Tests d'intégration légers : démarre un vrai serveur HTTP local (JDK
 * com.sun.net.httpserver, aucune dépendance supplémentaire nécessaire) qui
 * simule l'API /users, pour vérifier le comportement CRUD, la gestion des
 * codes non-2xx et le timeout explicite.
 */
class UserServiceImplTest {

    private HttpServer server;
    private URI baseUri;

    @BeforeEach
    void startServer() throws IOException {
        server = HttpServer.create(new InetSocketAddress("localhost", 0), 0);

        server.createContext("/users/1", exchange -> {
            String json = "{\"id\":1,\"name\":\"Alice\",\"email\":\"alice@example.com\"}";
            sendJson(exchange, 200, json);
        });
        server.createContext("/users/404", exchange -> sendJson(exchange, 404, "{\"error\":\"not found\"}"));
        server.createContext("/users/slow", exchange -> {
            try {
                TimeUnit.SECONDS.sleep(2);
            } catch (InterruptedException ignored) {
                Thread.currentThread().interrupt();
            }
            sendJson(exchange, 200, "{\"id\":99,\"name\":\"Trop lent\",\"email\":\"lent@example.com\"}");
        });
        server.createContext("/users", exchange -> {
            switch (exchange.getRequestMethod()) {
                case "GET" -> sendJson(exchange, 200,
                        "[{\"id\":1,\"name\":\"Alice\",\"email\":\"alice@example.com\"},"
                                + "{\"id\":2,\"name\":\"Bob\",\"email\":\"bob@example.com\"}]");
                case "POST" -> sendJson(exchange, 201, "{\"id\":3,\"name\":\"Carol\",\"email\":\"carol@example.com\"}");
                default -> sendJson(exchange, 405, "{}");
            }
        });

        server.start();
        baseUri = URI.create("http://localhost:" + server.getAddress().getPort() + "/");
    }

    @AfterEach
    void stopServer() {
        server.stop(0);
    }

    private static void sendJson(com.sun.net.httpserver.HttpExchange exchange, int status, String json) throws IOException {
        byte[] bytes = json.getBytes(StandardCharsets.UTF_8);
        exchange.getResponseHeaders().add("Content-Type", "application/json");
        exchange.sendResponseHeaders(status, bytes.length);
        try (var os = exchange.getResponseBody()) {
            os.write(bytes);
        }
    }

    @Test
    void getByIdAsync_succes_retourneUtilisateur() {
        UserService service = new UserServiceImpl(HttpClient.newHttpClient(), baseUri);
        User user = service.getByIdAsync("1").join();
        assertEquals(1, user.id());
        assertEquals("Alice", user.name());
    }

    @Test
    void getAllAsync_succes_retourneLaListe() {
        UserService service = new UserServiceImpl(HttpClient.newHttpClient(), baseUri);
        List<User> users = service.getAllAsync().join();
        assertEquals(2, users.size());
    }

    @Test
    void createAsync_succes_retourneUtilisateurCree() {
        UserService service = new UserServiceImpl(HttpClient.newHttpClient(), baseUri);
        User created = service.createAsync(new User(null, "Carol", "carol@example.com")).join();
        assertEquals(3, created.id());
    }

    @Test
    void getByIdAsync_codeNon2xx_leveUserServiceExceptionAvecStatutEtCorps() {
        UserService service = new UserServiceImpl(HttpClient.newHttpClient(), baseUri);
        CompletionException ex = assertThrows(CompletionException.class,
                () -> service.getByIdAsync("404").join());
        assertInstanceOf(UserServiceException.class, ex.getCause());
        UserServiceException use = (UserServiceException) ex.getCause();
        assertEquals(404, use.getStatusCode());
        assertTrue(use.getResponseBody().contains("not found"));
    }

    @Test
    void getByIdAsync_timeout_leveUserServiceTimeoutException() {
        // Timeout volontairement très court (200ms) pour ce test, la route /users/slow dort 2s.
        UserService service = new UserServiceImpl(HttpClient.newHttpClient(), baseUri, Duration.ofMillis(200));
        CompletionException ex = assertThrows(CompletionException.class,
                () -> service.getByIdAsync("slow").join());
        assertInstanceOf(UserServiceTimeoutException.class, ex.getCause());
    }
}
