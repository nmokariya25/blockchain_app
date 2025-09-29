using FluentValidation;
using MyBlockchain.Domain.Entities;

namespace MyBlockchain.Api.Validators
{
    public class DashBlockChainValidator : AbstractValidator<DashBlock>
    {
        public DashBlockChainValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class EthBlockChainValidator : AbstractValidator<EthBlock>
    {
        public EthBlockChainValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class BitCoinBlockChainValidator : AbstractValidator<BitCoinBlock>
    {
        public BitCoinBlockChainValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class BtcBlockChainValidator : AbstractValidator<BtcBlock>
    {
        public BtcBlockChainValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class LtcBlockChainValidator : AbstractValidator<LtcBlock>
    {
        public LtcBlockChainValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

}
