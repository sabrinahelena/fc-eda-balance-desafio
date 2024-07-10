using Microsoft.EntityFrameworkCore;

namespace Balance.Infra.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Balance.Domain.Entities.Balance> Balances { get; set; }
}
