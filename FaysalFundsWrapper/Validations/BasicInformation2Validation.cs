using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
   
    public class BasicInformation2Validation : AbstractValidator<BasicInformation2>
    {
        public BasicInformation2Validation()
        {
            RuleFor(x => x.DOB)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .MustBeValidDate();

            RuleFor(x => x.PlaceOfBirth)
                .NotEmpty().WithMessage("Place of Birth is required.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.");

            RuleFor(x => x.ResidentialStatus)
                .NotEmpty().WithMessage("Residential Status is required.");

            RuleFor(x => x.ZakatStatus)
                .NotEmpty().WithMessage("Zakat Status is required.");

            // ZakatStatus == "Liable" (case-insensitive)
            When(x => string.Equals(x.ZakatStatus, "liable", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.UploadCZ_50)
                    .Must(string.IsNullOrWhiteSpace)
                    .WithMessage("UploadCZ_50 must be empty when Zakat Status is 'Liable'.");

                //RuleFor(x => x.ReferralCode)
                //    .Must(string.IsNullOrWhiteSpace)
                //    .WithMessage("Referral Code must be empty when Zakat Status is 'Liable'.");
            });

            // ZakatStatus != "Liable" (case-insensitive)
            When(x => !string.Equals(x.ZakatStatus, "liable", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.UploadCZ_50)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("UploadCZ_50 is required when Zakat Status is not 'Liable'.")
                    .MustBeValidBase64();

            });
        }
    }

}
