namespace OrderService.Models;

/// <summary>
/// DTO immuable représentant un instantané en lecture seule d'une commande.
/// C'est ce type — et non l'entité <see cref="Order"/> suivie par EF Core —
/// qui doit être placé dans un cache partagé entre requêtes : un DTO n'est
/// attaché à aucun <c>DbContext</c> et ne peut donc jamais entrer en conflit
/// de tracking.
/// </summary>
/// <param name="Id">Identifiant de la commande.</param>
/// <param name="Status">Statut de la commande au moment de la capture.</param>
public record OrderSnapshot(Guid Id, OrderStatus Status)
{
    /// <summary>
    /// Construit un instantané immuable à partir de l'entité EF Core suivie.
    /// </summary>
    public static OrderSnapshot FromEntity(Order order) => new(order.Id, order.Status);
}
