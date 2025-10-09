using FaysalFundsWrapper.Models;
using FluentValidation;
namespace FaysalFundsWrapper.Validations;
public class TPinValidation
{
    public class GenerateTPinRequestValidator : AbstractValidator<GenerateTPinRequest>
    {
        public GenerateTPinRequestValidator()
        {
            RuleFor(x => x.Pin)
                .NotEmpty().WithMessage("T-PIN is required.")
                .Length(4).WithMessage("T-PIN must be exactly 4 digits.")
                .Matches(@"^\d+$").WithMessage("T-PIN must contain only digits.");
        }
    }

    public class VerifyTPinRequestValidator : AbstractValidator<VerifyTpinRequest>
    {
        public VerifyTPinRequestValidator()
        {
            RuleFor(x => x.Pin)
                .NotEmpty().WithMessage("T-PIN is required.")
                .Length(4).WithMessage("T-PIN must be exactly 4 digits.")
                .Matches(@"^\d+$").WithMessage("T-PIN must contain only digits.");
        }
    }
    public class ChangeTPinRequestValidator : AbstractValidator<ChangeTPinRequest>
    {
        public ChangeTPinRequestValidator()
        {
            
            RuleFor(x => x.OldPin)
                .NotEmpty().WithMessage("Old T-PIN is required.")
                .Length(4).WithMessage("Old T-PIN must be exactly 4 digits.")
                .Matches(@"^\d+$").WithMessage("Old T-PIN must contain only digits.");

            RuleFor(x => x.NewPin)
                .NotEmpty().WithMessage("New T-PIN is required.")
                .Length(4).WithMessage("New T-PIN must be exactly 4 digits.")
                .Matches(@"^\d+$").WithMessage("New T-PIN must contain only digits.");
        }
    }

    public class ForgotTPinRequestValidator : AbstractValidator<ForgotTPinRequest>
    {
        public ForgotTPinRequestValidator()
        {
                      RuleFor(x => x.NewPin)
                .NotEmpty().WithMessage("New T-PIN is required.")
                .Length(4).WithMessage("New T-PIN must be exactly 4 digits.")
                .Matches(@"^\d+$").WithMessage("New T-PIN must contain only digits.");
        }
    }
}
