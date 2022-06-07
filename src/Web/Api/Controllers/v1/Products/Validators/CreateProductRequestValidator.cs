using TariffComparison.Api.Controllers.v1.Products.Requests;
using FluentValidation;

namespace TariffComparison.Api.Controllers.v1.Products.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Consumption)
                .NotNull().GreaterThan(0).WithMessage("{PropertyName} is not valid");
        }
    }
}
