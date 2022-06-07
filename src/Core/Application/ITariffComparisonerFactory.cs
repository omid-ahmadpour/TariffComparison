using System.Collections.Generic;
using TariffComparison.Domain.Entities.Products;

namespace TariffComparison.Application
{
    public interface ITariffComparisonerFactory
    {
        IEnumerable<Product> Execute(int Consumption);
    }
}
