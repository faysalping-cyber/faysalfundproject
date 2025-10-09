using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
   
    public class QuickAccessValidation
    {
        // 1. Add Quick Access Menu
        public class AddQuickAccessMenuValidator : AbstractValidator<QuickAccesModel>
        {
            public AddQuickAccessMenuValidator()
            {
                RuleFor(x => x.NAME)
                    .NotEmpty().WithMessage("Menu name is required.")
                    .MaximumLength(100).WithMessage("Menu name must not exceed 100 characters.");

                RuleFor(x => x.ICON)
                    .NotNull().WithMessage("Menu icon is required.");

                RuleFor(x => x.ACTIVE)
                    .InclusiveBetween(0, 1).WithMessage("Active flag must be either 0 or 1.");
            }
        }

        // 2. Get User Quick Access Menus
        public class GetUserQuickAccessMenusValidator : AbstractValidator<long>
        {
            public GetUserQuickAccessMenusValidator()
            {
                RuleFor(x => x)
                    .GreaterThan(0).WithMessage("User ID must be valid.");
            }
        }

        //3. Add User Quick Access
        public class AddUserQuickAccessValidator : AbstractValidator<UserQuickAccessDto>
        {
            public AddUserQuickAccessValidator()
            {
              
                RuleFor(x => x.QUICKACCESSID)
                    .GreaterThan(0).WithMessage("Quick Access ID must be valid.");
            }
        }

        // 4. Remove User Quick Access
        public class RemoveUserQuickAccessValidator : AbstractValidator<UserQuickAccessDto>
        {
            public RemoveUserQuickAccessValidator()
            {
      
                RuleFor(x => x.QUICKACCESSID)
                    .GreaterThan(0).WithMessage("QuickAccessId is required for removal");
            }
        }
    }
}