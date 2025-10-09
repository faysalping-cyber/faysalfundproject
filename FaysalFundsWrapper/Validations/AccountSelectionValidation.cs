using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class AccountSelectionValidation:AbstractValidator<AccountSelectionRequestModel>
    { 
        public AccountSelectionValidation()
        {
            RuleFor(e=>e.AccountType).NotEmpty().NotNull().InclusiveBetween(1, 100);
        }
    }
}
