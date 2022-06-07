using Microsoft.EntityFrameworkCore;

namespace TariffComparison.Persistance.Db
{
    public class AppWriteDbContext : AppDbContext
    {
        public AppWriteDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public AppWriteDbContext() { }
    }
}