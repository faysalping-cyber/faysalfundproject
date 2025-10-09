using FluentValidation;

namespace FaysalFundsInternal.API.ModelValidations.ValidationUtils
{
    public static class ValidationUtils
    {
        public static IRuleBuilderOptions<T, string> MustBeValidCNIC<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .MinimumLength(15)
                .Matches(@"^\d{5}-\d{7}-\d{1}$")
                .WithMessage("CNIC must be in the format 12345-1234567-1.");
        }
    }
}
