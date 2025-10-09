using FaysalFundsInternal.API.ModelValidations.ValidationUtils;
using FaysalFundsInternal.Infrastructure.DTOs;
using FluentValidation;

namespace FaysalFundsInternal.API.ModelValidations
{
    public class LFDRequestModelValidation : AbstractValidator<LFDRequestModel>
    {
        public LFDRequestModelValidation()
        {
            RuleFor(x => x.CNIC).NotEmpty()
                .MustBeValidCNIC();
        }
    }
}
