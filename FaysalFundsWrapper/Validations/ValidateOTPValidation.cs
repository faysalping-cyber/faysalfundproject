using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class ValidateOTPValidation: AbstractValidator<ValidateOTPModel>
    {
        public ValidateOTPValidation()
        {
            RuleFor(e => e.EmailOTP).NotEmpty().NotNull().Length(4);
            RuleFor(e => e.CellOTP).NotEmpty().NotNull().Length(4);

        }
    }
}
