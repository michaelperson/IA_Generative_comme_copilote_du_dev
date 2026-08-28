package be.bstorm.formation.revue;

import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.util.List;
import java.util.concurrent.CompletableFuture;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

// Code proposé dans la Pull Request : récupération et traitement des commandes en attente.
public class OrderService {

    private final HttpClient httpClient;
    private final Gson gson = new Gson();

    public OrderService(HttpClient httpClient) {
        this.httpClient = httpClient;
    }

    // Récupère les commandes en attente depuis l'API et les traite
    public CompletableFuture<Integer> fetchAndProcessOrdersAsync() {
        return CompletableFuture.supplyAsync(() -> {
            int resultat = 0;

            for (int i = 0; i < 3; i++) {
                try {
                    HttpRequest request = HttpRequest.newBuilder()
                            .uri(URI.create("https://api.example.com/orders/pending"))
                            .build();
                    HttpResponse<String> response = httpClient.send(request, HttpResponse.BodyHandlers.ofString());
                    List<String> orders = gson.fromJson(response.body(), new TypeToken<List<String>>() {}.getType());
                    resultat += orders.size();
                    return resultat;
                } catch (Exception e) {
                }

                try {
                    Thread.sleep(30000);
                } catch (InterruptedException ignored) {
                }
            }

            return resultat;
        });
    }
}
