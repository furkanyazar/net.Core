using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.DbMigrationApplier;

public class DbMigrationApplierManager<TDbContext>(TDbContext context)
    : IDbMigrationApplierService<TDbContext>
    where TDbContext : DbContext
{
    private readonly TDbContext _context = context;

    public void Initialize()
    {
        _context.Database.EnsureDbApplied();
    }
}
