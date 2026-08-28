using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data;

/// <summary>
/// DbContext applicatif. Enregistré en <c>Scoped</c> dans le conteneur DI ASP.NET Core
/// (comportement par défaut d'AddDbContext) : une nouvelle instance par requête HTTP.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
}
