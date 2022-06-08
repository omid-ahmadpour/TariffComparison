using FluentValidation;
using TariffComparison.Api.Controllers.v1.Products.Requests;

namespace TariffComparison.Api.Controllers.v1.Products.Validators
{
    public class GetProductsRequestValidator : AbstractValidator<GetProductsRequest>
    {
        public GetProductsRequestValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("{PropertyName} is not valid");
        }
    }
}
