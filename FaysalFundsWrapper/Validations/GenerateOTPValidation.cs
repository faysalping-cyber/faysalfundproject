using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class GenerateOTPValidation : AbstractValidator<GenerateOTPModel>
    {
        public GenerateOTPValidation()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(x => x.Mobile).NotNull().NotEmpty().MustBeValidCellNumber();
            RuleFor(x => x.CountryCode).NotNull().NotEmpty().MustBeValidCountryCode();
            RuleFor(x => x.SameOtp).NotNull().NotEmpty();
            RuleFor(x => x.IsWhatsapp).NotNull().NotEmpty();


        }
    }
}
