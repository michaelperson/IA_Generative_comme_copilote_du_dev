package be.bstorm.formation.debugging.service;

import be.bstorm.formation.debugging.model.Order;
import be.bstorm.formation.debugging.model.OrderSnapshot;
import be.bstorm.formation.debugging.model.OrderStatus;
import be.bstorm.formation.debugging.repository.OrderRepository;
import com.github.benmanes.caffeine.cache.Cache;
import com.github.benmanes.caffeine.cache.Caffeine;
import java.util.NoSuchElementException;
import java.util.UUID;
import java.util.concurrent.TimeUnit;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/**
 * Version corrigée du service, voir DIAGNOSIS.md pour l'explication complète
 * de l'incident et le raisonnement menant à cette correction.
 *
 * <p>Principe du correctif : on ne met plus en cache l'entité {@link Order}
 * suivie par Hibernate (elle appartient à la session/EntityManager de la
 * requête qui l'a chargée, et devient invalide dès que cette session est
 * fermée en fin de requête). Le cache ne conserve désormais qu'un
 * {@link OrderSnapshot} immuable, et toute opération d'écriture recharge
 * systématiquement l'entité depuis le repository (donc depuis l'EntityManager
 * courant) avant de la modifier.</p>
 */
@Service
public class OrderService {

    private final OrderRepository orderRepository;
    private final Cache<UUID, OrderSnapshot> orderSnapshotCache = Caffeine.newBuilder()
            .expireAfterWrite(10, TimeUnit.MINUTES)
            .build();

    public OrderService(OrderRepository orderRepository) {
        this.orderRepository = orderRepository;
    }

    /**
     * Met à jour le statut d'une commande. L'entité est toujours rechargée
     * depuis le repository (EntityManager courant) : on n'essaie jamais de
     * réattacher une entité mise en cache par une requête précédente (et donc
     * potentiellement suivie par une session déjà fermée), ce qui éliminait la
     * cause de la {@code NonUniqueObjectException} observée en production.
     */
    @Transactional
    public void updateOrderStatus(UUID orderId, OrderStatus newStatus) {
        Order order = orderRepository.findById(orderId)
                .orElseThrow(() -> new NoSuchElementException("Commande '" + orderId + "' introuvable."));

        order.setStatus(newStatus);
        orderRepository.save(order);

        // Le cache ne conserve qu'un DTO immuable, jamais l'entité suivie par Hibernate.
        orderSnapshotCache.put(orderId, OrderSnapshot.fromEntity(order));
    }

    /**
     * Récupère un instantané en lecture seule d'une commande, en passant par
     * le cache applicatif. C'est le seul endroit où le cache est utilisé : il
     * ne sert que pour de la lecture, jamais pour réattacher une entité à des
     * fins d'écriture.
     */
    public OrderSnapshot getOrderSnapshot(UUID orderId) {
        return orderSnapshotCache.get(orderId,
                id -> orderRepository.findById(id).map(OrderSnapshot::fromEntity).orElse(null));
    }
}
