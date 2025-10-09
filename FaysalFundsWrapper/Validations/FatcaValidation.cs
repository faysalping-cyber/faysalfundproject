using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class FatcaValidation: AbstractValidator<FATCA>
    {
        public FatcaValidation()
        {
            RuleFor(e => e.CountryOfTaxResidenceElsePakistan)
                .NotEmpty().WithMessage("Country of Tax Residence is required.")
                .NotNull().WithMessage("Country of Tax Residence is required.")
                .MaximumLength(6).MinimumLength(3);

            // Validation for USA-specific fields (only when CountryOfTaxResidenceElsePakistan == "USA")
            When(e => e.CountryOfTaxResidenceElsePakistan == "USA", () =>
            {
                RuleFor(e => e.IsUS_CitizenResidentOrHaveGreenCard)
                    .NotNull().WithMessage("US citizenship/residency status is required.")
                    .InclusiveBetween(0, 1);

                RuleFor(e => e.IsUs_Born)
                    .NotNull().WithMessage("Birth information is required.")
                    .InclusiveBetween(0, 1);

                RuleFor(e => e.InstructionsToTransferFundsToUSA)
                    .NotNull().WithMessage("Instruction to transfer funds is required.")
                    .InclusiveBetween(0, 1);

                RuleFor(e => e.HaveUS_ResidenceMailingOrHoldingAddress)
                    .NotNull().WithMessage("Mailing address status is required.")
                    .InclusiveBetween(0, 1);

                RuleFor(e => e.HaveUS_TelephoneNumber)
                    .NotNull().WithMessage("US telephone number status is required.")
                    .InclusiveBetween(0, 1);

                RuleFor(e => e.US_TaxPayerIdentificationNumber)
    .NotEmpty().WithMessage("Taxpayer ID is required.")
    .Matches(@"^\d{3}-\d{2}-\d{4}$")
    .WithMessage("Taxpayer ID must be in the format 123-45-6789.");



                RuleFor(e => e.W9FormUpload)
                    .NotEmpty().WithMessage("W-9 form is required.")
                    .MustBeValidBase64().WithMessage("Invalid W-9 base64 content.");
            });

            // This field applies regardless of country, so validate always
            RuleFor(e => e.FATCA_Declaration)
                .NotNull().WithMessage("FATCA declaration is required.")
                .Equal(1);

            // Non-US Declaration is only required if country is "Other" or "None"
            When(e => e.CountryOfTaxResidenceElsePakistan == "Other" || e.CountryOfTaxResidenceElsePakistan == "None", () =>
            {
                RuleFor(e => e.NonUS_PersonDeclaration)
                    .NotNull().WithMessage("Non-US person declaration is required.")
                   .Equal(1);
            });

        }
    }
}
