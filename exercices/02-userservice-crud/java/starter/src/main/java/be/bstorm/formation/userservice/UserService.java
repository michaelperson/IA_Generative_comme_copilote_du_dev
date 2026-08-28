package be.bstorm.formation.userservice;

import java.util.List;
import java.util.concurrent.CompletableFuture;

/**
 * Contrat d'accès aux données pour la ressource /users de l'API REST externe.
 * Toutes les méthodes sont asynchrones (CompletableFuture, équivalent Java de
 * Task&lt;T&gt;). Aucune logique métier ne doit se trouver dans une implémentation
 * de cette interface : uniquement l'accès aux données (lecture/écriture via HTTP).
 *
 * <p>Note d'adaptation : Java n'a pas d'équivalent direct de {@code CancellationToken}.
 * Le timeout réseau (10 secondes) est géré en interne par l'implémentation via le
 * timeout de requête HTTP ; voir le commentaire dans {@link UserServiceImpl}.</p>
 */
public interface UserService {

    /** Récupère un utilisateur par son identifiant (GET /users/{id}). */
    CompletableFuture<User> getByIdAsync(String id);

    /** Récupère la liste complète des utilisateurs (GET /users). */
    CompletableFuture<List<User>> getAllAsync();

    /** Crée un nouvel utilisateur (POST /users). */
    CompletableFuture<User> createAsync(User user);

    /** Met à jour un utilisateur existant (PUT /users/{id}). */
    CompletableFuture<User> updateAsync(String id, User user);

    /** Supprime un utilisateur (DELETE /users/{id}). */
    CompletableFuture<Void> deleteAsync(String id);
}
