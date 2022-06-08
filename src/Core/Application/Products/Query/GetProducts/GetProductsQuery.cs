using MediatR;
using System.Collections.Generic;

namespace TariffComparison.Application.Products.Query.GetProducts
{
    public class GetProductsQuery : IRequest<IEnumerable<ProductQueryModel>>
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }
}