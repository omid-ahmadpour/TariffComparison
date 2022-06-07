using Microsoft.EntityFrameworkCore;

namespace TariffComparison.Persistance.Db
{
    public class AppReadOnlyDbContext : AppDbContext
    {
        public AppReadOnlyDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
