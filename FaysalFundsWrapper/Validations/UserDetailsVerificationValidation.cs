using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class UserDetailsVerificationValidation : AbstractValidator<UserDetailsVerification>
    {
        public UserDetailsVerificationValidation()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.CNIC).MustBeValidCNIC();
            RuleFor(x => x.PhoneNo).NotNull().NotEmpty().MustBeValidCellNumber();
            RuleFor(x => x.CountryCode).MustBeValidCountryCode();

        }
    }
}
