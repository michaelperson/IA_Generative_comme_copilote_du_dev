package be.bstorm.formation.debugging.model;

import java.util.UUID;

/**
 * DTO immuable représentant un instantané en lecture seule d'une commande.
 * C'est ce type — et non l'entité {@link Order} suivie par la session
 * Hibernate — qui doit être placé dans un cache applicatif partagé entre
 * requêtes : un DTO n'est attaché à aucune session et ne peut donc jamais
 * entrer en conflit de tracking (NonUniqueObjectException).
 */
public record OrderSnapshot(UUID id, OrderStatus status) {

    /** Construit un instantané immuable à partir de l'entité JPA suivie. */
    public static OrderSnapshot fromEntity(Order order) {
        return new OrderSnapshot(order.getId(), order.getStatus());
    }
}
