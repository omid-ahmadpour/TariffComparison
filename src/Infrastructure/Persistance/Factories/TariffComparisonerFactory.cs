using System;
using System.Collections.Generic;
using System.Linq;
using TariffComparison.Application;
using TariffComparison.Common.General;
using TariffComparison.Domain.Entities.Products;

namespace TariffComparison.Persistance.Factories
{
    public class TariffComparisonerFactory : ITariffComparisonerFactory
    {
        public IEnumerable<Product> Execute(int consumption)
        {
            var products = new List<Product>()
            {
                new Product
                {
                    TariffName = CalculationType.BasicElectricityTariff.ToDisplay(),
                    AnnualCosts = BasicElectricityTariffCalculation(consumption)
                },
                new Product
                {
                    TariffName = CalculationType.PackagedTariff.ToDisplay(),
                    AnnualCosts = PackagedTariffCalculation(consumption)
                }
            };

            return products.OrderBy(field => field.AnnualCosts);
        }

        private static int BasicElectricityTariffCalculation(int consumption)
        {
            return Convert.ToInt32(Math.Round(Convert.ToDecimal((5 * 12) + ((22 * consumption) / 100))));
        }

        private static int PackagedTariffCalculation(int consumption)
        {
            if (consumption <= 4000)
            {
                return 800;
            }
            else
            {
                return 800 + (((consumption - 4000) * 30) / 100);
            }
        }
    }
}
