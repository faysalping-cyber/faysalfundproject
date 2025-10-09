using FaysalFundsWrapper.Models.AccountOpening;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class OnBoardigSubmitValidation : AbstractValidator<OnboardingSubmitStatusDTO>
    {

        public OnBoardigSubmitValidation()
        {
            RuleFor(x => x.STATUS)
     .NotNull().WithMessage("STATUS is required.")
     .InclusiveBetween(0, int.MaxValue).WithMessage("STATUS must be a non-negative integer.");
        }

    }
}
