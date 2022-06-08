using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TariffComparison.Application.Products.Query.GetProducts;
using TariffComparison.Domain.Entities.Products;
using TariffComparison.Persistance.Db;

namespace TariffComparison.Persistance.QueryHandlers.Products
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductQueryModel>>
    {
        private readonly AppReadOnlyDbContext _dbContext;

        public GetProductsQueryHandler(AppReadOnlyDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<ProductQueryModel>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _dbContext.Set<Product>()
                .OrderBy(field => field.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize).Select(a =>
               new ProductQueryModel
               {
                   Id = a.Id,
                   TariffName = a.TariffName,
                   AnnualCosts = a.AnnualCosts
               }).ToListAsync(cancellationToken: cancellationToken);

            return products;
        }
    }
}