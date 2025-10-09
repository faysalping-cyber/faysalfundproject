using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class RegulatoryKYCStep2Validation:AbstractValidator<RegulatoryKYC2>
    {
        public RegulatoryKYCStep2Validation()
        {
            RuleFor(e => e.FinantialInstitutionRefusal).NotEmpty().NotNull().InclusiveBetween(0, 1);
            RuleFor(e => e.UltimateBeneficiary).NotEmpty().NotNull().InclusiveBetween(0, 1);
            RuleFor(e => e.HoldingSeniorPosition).NotEmpty().NotNull().InclusiveBetween(0, 1);
            RuleFor(e => e.InternationalRelationshipOrPREP).NotEmpty().NotNull().InclusiveBetween(0, 1);
            RuleFor(e => e.DealingWithHighValueItems).NotEmpty().NotNull().InclusiveBetween(0, 1);
            RuleFor(e => e.FinancialLinksWithOffshoreTaxHavens).NotEmpty().NotNull().InclusiveBetween(0, 1);
            RuleFor(e => e.TrueInformationDeclaration).NotEmpty().NotNull().InclusiveBetween(0, 1);
        }
    }
}
