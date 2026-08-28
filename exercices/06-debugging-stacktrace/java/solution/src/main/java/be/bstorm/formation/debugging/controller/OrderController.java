package be.bstorm.formation.debugging.controller;

import be.bstorm.formation.debugging.model.OrderStatus;
import be.bstorm.formation.debugging.service.OrderService;
import java.util.UUID;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

/**
 * Contrôleur minimal reproduisant le point d'entrée mentionné dans la stack
 * trace de l'incident (OrderController.java:line 22).
 */
@RestController
@RequestMapping("/api/orders")
public class OrderController {

    private final OrderService orderService;

    public OrderController(OrderService orderService) {
        this.orderService = orderService;
    }

    @PatchMapping("/{id}/status")
    public void patchStatus(@PathVariable UUID id, @RequestBody OrderStatus status) {
        orderService.updateOrderStatus(id, status);
    }
}
