using FaysalFundsWrapper.Models;
using FluentValidation;
using System.Globalization;

namespace FaysalFundsWrapper.Validations
{
    public class BasicInformation1Validation : AbstractValidator<BasicInformation1>
    {
        private bool BeValidDate(string? date)
        {
            return DateTime.TryParseExact(
                date,
                "dd-MM-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _);
        }

        public BasicInformation1Validation()
        {
            RuleFor(x => x.FullName).MustBeValidName();
            RuleFor(x => x.FatherOrHusbandName).MustBeValidName();
            RuleFor(x => x.MotherName).MustBeValidName();
            RuleFor(x => x.CNIC).MustBeValidCNIC();
            RuleFor(x => x.CNIC_IssueDate)
                .NotNull().WithMessage("Issue date is required.")
                .NotEmpty().WithMessage("Issue date cannot be empty.")
                .MustBeValidDate();

            // If not lifetime, expiry date is required
            When(x => x.IsCnicExpiryLifetime == 0, () =>
            {
                RuleFor(x => x.CNIC_ExpiryDate)
           .NotEmpty().WithMessage("CNIC expiry date is required when CNIC is not lifetime.")
           .Must(BeValidDate).WithMessage("CNIC expiry date must be a valid date (dd-MM-yyyy).")
           .WithName("CNIC Expiry Date");
            });
            // If lifetime, expiry date must be null or empty
            When(x => x.IsCnicExpiryLifetime == 1, () =>
            {
                RuleFor(x => x.CNIC_ExpiryDate)
        .Must(string.IsNullOrWhiteSpace)
        .WithMessage("CNIC expiry date must be empty when CNIC is lifetime.")
        .WithName("CNIC Expiry Date");
            });
        }


    }
}
