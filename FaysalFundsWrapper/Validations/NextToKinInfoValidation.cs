using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class NextToKinInfoValidation: AbstractValidator<NextToKinInfo>
    {
        public NextToKinInfoValidation()
        {
            RuleFor(x => x.NextOfKinName).MustBeValidName().NotNull().NotEmpty();
            RuleFor(x => x.CountryCode).MustBeValidCountryCode().NotNull().NotEmpty();
            RuleFor(x => x.ContactNo).MustBeValidCellNumber().NotNull().NotEmpty();

        }
    }
}
