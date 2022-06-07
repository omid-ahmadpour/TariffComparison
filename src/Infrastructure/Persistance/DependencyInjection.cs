﻿namespace TariffComparison.Persistance
{
    using TariffComparison.Common.General;
    using TariffComparison.Persistance.Db;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using TariffComparison.Persistance.Factories;
    using TariffComparison.Application;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            var appOptions = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped((serviceProvider) =>
            {
                var option = CreateContextOptions(appOptions.ReadDatabaseConnectionString);
                return new AppReadOnlyDbContext(option);
            });

            services.AddScoped((serviceProvider) =>
            {
                var option = CreateContextOptions(appOptions.WriteDatabaseConnectionString);
                return new AppWriteDbContext(option);
            });

            DbContextOptions<AppDbContext> CreateContextOptions(string connectionString)
            {
                var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                     .UseSqlServer(connectionString)
                                     .Options;

                return contextOptions;
            }

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appOptions.WriteDatabaseConnectionString));

            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<ITariffComparisonerFactory, TariffComparisonerFactory>();

            return services;
        }
    }
}
