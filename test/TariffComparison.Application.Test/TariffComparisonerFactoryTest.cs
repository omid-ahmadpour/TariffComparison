using System.Linq;
using TariffComparison.Common.General;
using TariffComparison.Persistance.Factories;
using Xunit;

namespace TariffComparison.Infrastructure.Test
{
    public class TariffComparisonerFactoryTest
    {
        [Theory]
        [InlineData(3500, 830)]
        [InlineData(4500, 1050)]
        [InlineData(6000, 1380)]
        public void BasicElectricityTariffCalculateComparisonTest(int consumption, int annualCost)
        {
            //arrange
            var factory = new TariffComparisonerFactory();

            //act
            var result = factory.Execute(consumption);

            //assert
            var terrifProduct = result.FirstOrDefault(item => item.TariffName == CalculationType.BasicElectricityTariff.ToDisplay());

            Assert.Equal(annualCost, terrifProduct?.AnnualCosts);
        }

        [Theory]
        [InlineData(3500, 800)]
        [InlineData(4500, 950)]
        [InlineData(6000, 1400)]
        public void PackagedTariffCalculateComparisonTest(int consumption, int annualCost)
        {
            //arrange
            var factory = new TariffComparisonerFactory();

            //act
            var result = factory.Execute(consumption);

            //assert
            var terrifProduct = result.FirstOrDefault(item => item.TariffName == CalculationType.PackagedTariff.ToDisplay());

            Assert.Equal(annualCost, terrifProduct?.AnnualCosts);
        }
    }
}