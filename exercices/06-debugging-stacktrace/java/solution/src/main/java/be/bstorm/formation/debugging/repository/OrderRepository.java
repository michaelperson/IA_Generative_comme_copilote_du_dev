package be.bstorm.formation.debugging.repository;

import be.bstorm.formation.debugging.model.Order;
import java.util.UUID;
import org.springframework.data.jpa.repository.JpaRepository;

public interface OrderRepository extends JpaRepository<Order, UUID> {
}
