using FluentValidation;
using MyBlockchain.Domain.Entities;

namespace MyBlockchain.Api.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}
