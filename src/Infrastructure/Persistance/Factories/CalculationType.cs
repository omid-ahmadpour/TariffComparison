using System.ComponentModel.DataAnnotations;

namespace TariffComparison.Persistance.Factories
{
    public enum CalculationType
    {
        [Display(Name = "Basic electricity tariff")]
        BasicElectricityTariff,

        [Display(Name = "Packaged tariff")]
        PackagedTariff
    }

    
}
