using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class RiskProfileValidation: AbstractValidator<RiskProfile>
    {
        public RiskProfileValidation()
        {
            RuleFor(x => x.Age).NotEmpty().NotNull().InclusiveBetween(1, 50); 
            RuleFor(x => x.Education).NotEmpty().NotNull().InclusiveBetween(1, 50);
            RuleFor(x => x.NoOfDependents).NotEmpty().NotNull().InclusiveBetween(1, 50);
            RuleFor(x => x.RiskAppetite).NotEmpty().NotNull().InclusiveBetween(1, 50);
            RuleFor(x => x.InvestmentHorizon).NotEmpty().NotNull().InclusiveBetween(1, 50);
            RuleFor(x => x.InvestmentObjective).NotEmpty().NotNull().InclusiveBetween(1, 50);
            RuleFor(x => x.InvestmentKnowledge).NotEmpty().NotNull().InclusiveBetween(1, 50);
            RuleFor(x => x.FinancialPosition).NotEmpty().NotNull().InclusiveBetween(1, 50);
            RuleFor(x => x.MaritalStatus).NotEmpty().NotNull().InclusiveBetween(1, 50);
            RuleFor(x => x.Occupation).NotEmpty().NotNull().InclusiveBetween(1, 50);
            RuleFor(x => x.RiskProfileDeclaration).NotEmpty().NotNull().InclusiveBetween(0, 1);

        }
    }
}
