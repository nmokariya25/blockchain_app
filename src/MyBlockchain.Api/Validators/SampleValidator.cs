using FluentValidation;
using MyBlockchain.Domain.Entities;

namespace MyBlockchain.Api.Validators
{
    public class SampleValidator : AbstractValidator<EthBlock>
    {
        public SampleValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
