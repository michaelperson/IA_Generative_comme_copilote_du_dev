// OrderService.cs
// Extrait de code source de l'incident (à fournir en contexte) — reproduit tel quel
// depuis la classe originale.
//
// Contexte production : cette exception survient de façon intermittente,
// uniquement sous charge (plusieurs requêtes concurrentes sur la même commande).
// Le DbContext est enregistré en Scoped (AddDbContext par défaut d'ASP.NET Core),
// l'IMemoryCache est un Singleton partagé par toute l'application.

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Services;

/// <summary>
/// BUG connu (à des fins pédagogiques) : voir STACKTRACE.txt et DIAGNOSTIC.md
/// dans ce même dossier. Cette classe est volontairement fautive — ne pas la
/// corriger avant d'avoir terminé l'étape 3 de l'exercice.
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
    /// Met à jour le statut d'une commande. Passe par un cache mémoire pour
    /// éviter une lecture base de données à chaque appel.
    /// </summary>
    public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
    {
        var order = await _cache.GetOrCreateAsync(orderId, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(10);
            return await _dbContext.Orders.FindAsync(orderId);
        });

        order.Status = newStatus;
        _dbContext.Orders.Update(order); // ligne 47
        await _dbContext.SaveChangesAsync();
    }
}
