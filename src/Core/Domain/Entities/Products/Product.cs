namespace TariffComparison.Domain.Entities.Products
{
    public class Product : IEntity<long>
    {
        public long Id { get; set; }

        public string TariffName { get; set; }

        public int AnnualCosts { get; set; }
    }
}
