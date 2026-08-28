using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Services;

/// <summary>
/// Version corrigée du service, voir DIAGNOSIS.md pour l'explication complète
/// de l'incident et le raisonnement menant à cette correction.
///
/// Principe du correctif : on ne met plus en cache l'entité <see cref="Order"/>
/// suivie par EF Core (elle appartient au <c>DbContext</c> de la requête qui
/// l'a chargée, et devient invalide dès que ce <c>DbContext</c> est disposé en
/// fin de requête). Le cache ne conserve désormais qu'un <see cref="OrderSnapshot"/>
/// immuable, et toute opération d'écriture recharge systématiquement l'entité
/// depuis le <c>DbContext</c> courant avant de la modifier.
/// </summary>
public class OrderService
{
    private readonly IMemoryCache _cache;
    private readonly AppDbContext _dbContext;

    public OrderService(IMemoryCache cache, AppDbContext dbContext)
    {
        _cache = cache;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Met à jour le statut d'une commande.
    /// </summary>
    /// <remarks>
    /// L'entité est toujours rechargée depuis le <c>DbContext</c> courant : on
    /// n'essaie jamais de réattacher une entité mise en cache par une requête
    /// précédente (et donc suivie par un autre <c>DbContext</c>, potentiellement
    /// déjà disposé), ce qui éliminait la cause de l'exception
    /// <c>InvalidOperationException</c> observée en production.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Levée si la commande n'existe pas.
    /// </exception>
    public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
    {
        var order = await _dbContext.Orders.FindAsync(orderId)
            ?? throw new InvalidOperationException($"Commande '{orderId}' introuvable.");

        order.Status = newStatus;
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();

        // Le cache ne conserve qu'un DTO immuable, jamais l'entité trackée par EF Core.
        _cache.Set(orderId, OrderSnapshot.FromEntity(order), TimeSpan.FromMinutes(10));
    }

    /// <summary>
    /// Récupère un instantané en lecture seule d'une commande, en passant par
    /// le cache mémoire. C'est le seul endroit où le cache est utilisé : il ne
    /// sert que pour de la lecture, jamais pour réattacher une entité à des
    /// fins d'écriture.
    /// </summary>
    public async Task<OrderSnapshot?> GetOrderSnapshotAsync(Guid orderId)
    {
        return await _cache.GetOrCreateAsync(orderId, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(10);
            var order = await _dbContext.Orders.FindAsync(orderId);
            return order is null ? null : OrderSnapshot.FromEntity(order);
        });
    }
}
