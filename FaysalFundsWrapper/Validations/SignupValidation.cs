using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class SignupValidation: AbstractValidator<SignupRequest>
    {
        public SignupValidation()
        {
            RuleFor(x => x.Email).EmailAddress().NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty().StrongPassword();
            RuleFor(x => x.CountryCode).MustBeValidCountryCode();
            RuleFor(x => x.PhoneNo).NotNull().NotEmpty().MustBeValidCellNumber();
            RuleFor(x => x.Name).MustBeValidName();
            RuleFor(x => x.CNIC).MustBeValidCNIC();


        }
    }
}
