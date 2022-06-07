namespace TariffComparison.Persistance.Db
{
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;
    using TariffComparison.Common.Utilities;
    using TariffComparison.Domain.Entities;

    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public AppDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IEntity).Assembly;

            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IEntity).Assembly);
            modelBuilder.AddPluralizingTableNameConvention();
        }

        public async Task<int> ExecuteSqlRawAsync(string query, CancellationToken cancellationToken)
        {
            var result = await base.Database.ExecuteSqlRawAsync(query, cancellationToken);
            return result;
        }

        public async Task<int> ExecuteSqlRawAsync(string query) => await ExecuteSqlRawAsync(query, CancellationToken.None);
    }
}
