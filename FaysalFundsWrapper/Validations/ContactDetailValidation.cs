using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class ContactDetailValidation : AbstractValidator<ContactDetail>
    {
        public ContactDetailValidation(IOnBoardingService onBoardingService)
        {
            RuleFor(x => x.PermanentAddress)
                .NotEmpty()
                .MaximumLength(500)
                .WithMessage("Permanent address is required and must be less than 500 characters.");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Country is required.");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required.");

            RuleFor(x => x.MailingAddressSameAsPermanent)
                .NotNull()
                .InclusiveBetween(0, 1)
                .WithMessage("Mailing address same as permanent must be 0 or 1.");

            RuleFor(x => x.CountryCode)
                .NotEmpty()
                .MustBeValidCountryCode()
                .WithMessage("A valid country code is required.");

            RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .MustBeValidCellNumber()
                .WithMessage("A valid mobile number is required.");

            // Apply mailing address validation only when MailingAddressSameAsPermanent is FALSE (0)
            When(x => x.MailingAddressSameAsPermanent == 0, () =>
            {
                RuleFor(x => x.MalingCity)
                    .Must(city => city != null && city != 0)
                    .WithMessage("MalingCity: Mailing city is required and must be a valid ID when mailing address is different from permanent address.");

                RuleFor(x => x.MalingCountry)
                    .Must(country => country != null && country != 0)
                    .WithMessage("MalingCountry: Mailing country is required and must be a valid ID when mailing address is different from permanent address.");

                RuleFor(x => x.MalingAddress1)
                    .NotEmpty()
                    .WithMessage("Mailing address is required when mailing address is different from permanent address.");
            });

            // MobileNumberOwnership validation based on GetSavedAccountSelection()
            //RuleFor(x => x.MobileNumberOwnership)
            //    .NotNull()

            //    .WhenAsync(async(x, context) =>
            //    {
            //        var result = await onBoardingService.GetSavedAccountSelection();
            //        return result == 2;
            //    }).WithMessage("Mobile number ownership is required in Digital Sarmayakari Account.");
        }
    }
}
