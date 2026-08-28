using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data;

/// <summary>
/// DbContext applicatif, enregistré en <c>Scoped</c> (comportement par défaut
/// d'<c>AddDbContext</c>) : une nouvelle instance par requête HTTP.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
}
