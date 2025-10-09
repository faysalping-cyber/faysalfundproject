using FluentValidation;
using FluentValidation.Results;

namespace FaysalFundsWrapper.Validations
{
    public static class ValidationExtensions
    {
        public static void ValidateAndThrow<T>(this T instance, IValidator<T> validator)
        {
            ValidationResult result = validator.Validate(instance);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }


    }
}
