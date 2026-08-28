package be.bstorm.formation.revue;

import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.util.List;
import java.util.concurrent.CancellationException;
import java.util.concurrent.CompletableFuture;
import java.util.function.BooleanSupplier;

// Jackson est la bibliothèque JSON déjà utilisée ailleurs dans le projet ;
// elle remplace ici Gson (nouvelle dépendance non justifiée dans la version relue).
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 * Récupère les commandes en attente depuis l'API des commandes et les traite.
 */
public class CorrectedExample {

    private static final Logger LOGGER = LoggerFactory.getLogger(CorrectedExample.class);

    // Nombre de tentatives avant abandon en cas d'échec de l'appel API.
    private static final int MAX_RETRY_ATTEMPTS = 3;

    // Délai d'attente entre deux tentatives (en millisecondes).
    private static final long RETRY_DELAY_MILLIS = 30_000L;

    private final HttpClient httpClient;
    private final ObjectMapper objectMapper = new ObjectMapper();

    public CorrectedExample(HttpClient httpClient) {
        this.httpClient = httpClient;
    }

    /**
     * Récupère les commandes en attente depuis l'API et retourne le nombre de commandes traitées.
     *
     * @param isCancelled équivalent Java d'un CancellationToken : permet à l'appelant de demander
     *                    l'arrêt coopératif de l'opération entre deux tentatives.
     */
    public CompletableFuture<Integer> fetchAndProcessOrdersAsync(BooleanSupplier isCancelled) {
        return CompletableFuture.supplyAsync(() -> {
            int processedCount = 0;

            for (int attempt = 0; attempt < MAX_RETRY_ATTEMPTS; attempt++) {
                if (isCancelled.getAsBoolean()) {
                    throw new CancellationException("Opération annulée par l'appelant");
                }

                try {
                    HttpRequest request = HttpRequest.newBuilder()
                            .uri(URI.create("https://api.example.com/orders/pending"))
                            .build();
                    HttpResponse<String> response = httpClient.send(request, HttpResponse.BodyHandlers.ofString());
                    List<?> orders = objectMapper.readValue(response.body(), List.class);
                    processedCount += orders.size();
                    return processedCount;
                } catch (Exception e) {
                    LOGGER.error(
                            "Échec de la tentative {}/{} de récupération des commandes en attente",
                            attempt + 1, MAX_RETRY_ATTEMPTS, e);
                }

                try {
                    Thread.sleep(RETRY_DELAY_MILLIS);
                } catch (InterruptedException e) {
                    Thread.currentThread().interrupt();
                    LOGGER.warn("Attente entre tentatives interrompue", e);
                }
            }

            return processedCount;
        });
    }
}
