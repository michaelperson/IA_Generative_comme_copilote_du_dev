// OrderService.java
// Extrait de code source de l'incident (à fournir en contexte) — adaptation
// Java/Spring Data JPA du même incident que la version C#/EF Core de cet
// exercice (voir README.md à la racine de l'exercice).
//
// Contexte production : cette exception survient de façon intermittente,
// uniquement sous charge (plusieurs requêtes concurrentes sur la même commande).
// L'EntityManager (via le repository Spring Data JPA) est lié au scope de la
// requête HTTP ; le cache Caffeine ci-dessous est un singleton applicatif,
// injecté une seule fois et partagé par toutes les requêtes.

package be.bstorm.formation.debugging.service;

import be.bstorm.formation.debugging.model.Order;
import be.bstorm.formation.debugging.model.OrderStatus;
import be.bstorm.formation.debugging.repository.OrderRepository;
import com.github.benmanes.caffeine.cache.Cache;
import com.github.benmanes.caffeine.cache.Caffeine;
import java.util.UUID;
import java.util.concurrent.TimeUnit;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/**
 * BUG connu (à des fins pédagogiques) : voir STACKTRACE.txt et DIAGNOSTIC.md
 * dans ce même dossier. Cette classe est volontairement fautive — ne pas la
 * corriger avant d'avoir terminé l'étape 3 de l'exercice.
 */
@Service
public class OrderService {

    private final OrderRepository orderRepository;
    private final Cache<UUID, Order> orderCache = Caffeine.newBuilder()
            .expireAfterWrite(10, TimeUnit.MINUTES)
            .build();

    public OrderService(OrderRepository orderRepository) {
        this.orderRepository = orderRepository;
    }

    @Transactional
    public void updateOrderStatus(UUID orderId, OrderStatus newStatus) {
        Order order = orderCache.get(orderId,
                id -> orderRepository.findById(id).orElseThrow());

        order.setStatus(newStatus);
        orderRepository.save(order); // ligne 47
    }
}
