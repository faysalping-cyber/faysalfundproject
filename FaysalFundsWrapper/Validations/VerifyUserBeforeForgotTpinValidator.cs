using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class VerifyUserBeforeForgotTpinValidator : AbstractValidator<VerifyUserBeforeForgotTpin>
    {
        public VerifyUserBeforeForgotTpinValidator()
        {
      
            RuleFor(x => x.Cnic)
                .NotEmpty().WithMessage("is required.")
                .Matches(@"^\d{5}-\d{7}-\d{1}$").WithMessage("CNIC format is invalid.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.CountryCode)
                .NotEmpty().WithMessage("is required.")
                .Matches(@"^\+\d{1,4}$").WithMessage("Invalid country code format.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("is required.")
                .Matches(@"^\d{10,15}$").WithMessage("Invalid phone number format.");
        }
    }
}
