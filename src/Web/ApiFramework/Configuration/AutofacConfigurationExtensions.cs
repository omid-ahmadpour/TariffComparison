using Autofac;
using TariffComparison.Common;
using TariffComparison.Common.General;
using TariffComparison.Domain.Entities;
using TariffComparison.Domain.IRepositories;
using TariffComparison.Persistance.Db;
using TariffComparison.Persistance.Repositories;

namespace TariffComparison.ApiFramework.Configuration
{
    public static class AutofacConfigurationExtensions
    {
        public static void RegisterServices(this ContainerBuilder containerBuilder)
        {
            //RegisterType > As > Liftetime
            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            containerBuilder.RegisterGeneric(typeof(EfReadOnlyRepository<>)).As(typeof(IReanOnlyRepository<>)).InstancePerLifetimeScope();

            var commonAssembly = typeof(SiteSettings).Assembly;
            var entitiesAssembly = typeof(IEntity).Assembly;
            var dataAssembly = typeof(AppDbContext).Assembly;

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly)
                .AssignableTo<IScopedDependency>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly)
                .AssignableTo<ITransientDependency>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly)
                .AssignableTo<ISingletonDependency>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
