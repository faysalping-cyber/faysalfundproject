using FaysalFundsWrapper.Models.Raast;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class GenerateRaastIdRequestModelValidation : AbstractValidator<GenerateRaastIdRequestModel>
    {
        public GenerateRaastIdRequestModelValidation()
        {
            RuleFor(x => x.TPin)
               .NotEmpty().WithMessage("T-PIN is required.")
               .Length(4).WithMessage("T-PIN must be exactly 4 digits.")
               .Matches(@"^\d+$").WithMessage("T-PIN must contain only digits.");

            RuleFor(x => x.FolioNo)
                .NotEmpty().WithMessage("Folio is required.")
                .Matches(@"^\d+$").WithMessage("Folio must contain only digits.");

            RuleFor(x => x.FundCode)
                .NotEmpty().WithMessage("Fund code is required.");
        }
    }
}
   