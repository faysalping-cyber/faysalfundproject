using FaysalFundsWrapper.Models;
using FluentValidation;
using System.Linq;

namespace FaysalFundsWrapper.Validations
{
    public class RegulatoryKYCStep1Validation : AbstractValidator<RegulatoryKYC1>
    {
        public RegulatoryKYCStep1Validation()
        {
            var nullOccupationIds = new List<int?> { 3, 4, 5, 6 };

            RuleFor(e => e.Profession)
                .NotNull().WithMessage("Profession is required.")
                .GreaterThan(0).WithMessage("Profession must be greater than 0.")
                .When(e => !nullOccupationIds.Contains(e.Occupation));


                    RuleFor(e => e.NameOfEmployerOrBussiness)
            .NotEmpty().WithMessage("Name of Employer or Business is required.")
            .When(e => !nullOccupationIds.Contains(e.Occupation)); // Skip if in the list


            RuleFor(e => e.SourceOfIncome)
                .NotEmpty().WithMessage("Source of Income is required.")
                .NotNull()
                .InclusiveBetween(1, 50);

            RuleFor(e => e.GrossAnnualIncome)
                .NotNull()
                .GreaterThan(0)
                .LessThanOrEqualTo(1_000_000_000);

            RuleFor(e => e.MonthlyExpectedInvestmentAmount)
                .NotNull()
                .GreaterThan(0)
                .LessThanOrEqualTo(100_000);

            RuleFor(e => e.MonthlyExpectedNoOfInvestmentTransaction)
                .NotNull()
                .InclusiveBetween(1, 50);

            RuleFor(e => e.MonthlyExpectedNoOfRedemptionTransaction)
                .NotNull()
                .InclusiveBetween(1, 50);

            RuleFor(e => e.Occupation)
                .NotNull()
                .InclusiveBetween(1, 50);
        }
    }

}
