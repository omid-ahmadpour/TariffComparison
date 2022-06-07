using MediatR;

namespace TariffComparison.Application.Products.Command.CreateProduct
{
    public class CreateProductCommand : IRequest<int>
    {
        public int Consumption { get; set; }
    }
}
