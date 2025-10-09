using FluentValidation;
using System.Globalization;

namespace FaysalFundsWrapper.Validations
{
    public static class ValidationUtils
    {
 
            public static IRuleBuilderOptions<T, string> MustBeValidDate<T>(
                this IRuleBuilder<T, string> ruleBuilder,
                string format = "d-M-yyyy")
            {
                return ruleBuilder.Must(date =>
                {
                    return DateTime.TryParseExact(
                        date,
                        format,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out _);
                })
                .WithMessage($"Date must be in the format {format}");
            }
        


        public static IRuleBuilderOptions<T, string> MustBeValidBase64<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value =>
            {
                if (string.IsNullOrWhiteSpace(value))
                    return false;

                try
                {
                    Convert.FromBase64String(value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }).WithMessage("The value must be a valid Base64-encoded string.");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidCountryCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotNull()
                .NotEmpty()
                .Matches(@"^\+\d{1,3}$")
                .WithMessage("Country code must start with '+' followed by 1 to 3 digits.");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidCellNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(@"^\d{7,12}$")
                .WithMessage("Cell number must be between 7 and 12 digits (numbers only).");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(150)
                .Matches("^[A-Za-z ]+$")
                .WithMessage("Name must only contain letters and spaces (3–150 characters).");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidCNIC<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotNull()
                .NotEmpty()
                .MinimumLength(15)
                .Matches(@"^\d{5}-\d{7}-\d{1}$")
                .WithMessage("CNIC must be in the format 12345-1234567-1.");
        }

        public static IRuleBuilderOptions<T, string> StrongPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotNull()
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(12).WithMessage("must be at least 12 characters long.")
                .Matches(@"[A-Z]").WithMessage("must contain at least one uppercase letter.")
                .Matches(@"\d").WithMessage("must contain at least one number.")
                .Matches(@"[\W_]").WithMessage("must contain at least one special character.");
        }

    }
}