using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TariffComparison.Application;
using TariffComparison.Application.Products.Command.CreateProduct;
using TariffComparison.Common.Utilities;
using TariffComparison.Domain.Entities.Products;
using TariffComparison.Persistance.Db;

namespace TariffComparison.Persistance.CommandHandlers.Products
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly AppWriteDbContext _dbContext;
        private readonly ILogger<CreateProductCommandHandler> _logger;
        private readonly ITariffComparisonerFactory _tariffComparisonerFactory;

        public CreateProductCommandHandler(AppWriteDbContext dbContext,
                                           ILogger<CreateProductCommandHandler> logger,
                                           ITariffComparisonerFactory tariffComparisonerFactory)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tariffComparisonerFactory = tariffComparisonerFactory ?? throw new ArgumentNullException(nameof(tariffComparisonerFactory));
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            GuardClauses.IsNotNull(request, nameof(request));

            var products = _tariffComparisonerFactory.Execute(request.Consumption);

            await _dbContext.Set<Product>().AddRangeAsync(products, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product Inserted");
            return request.Consumption;
        }
    }
}
