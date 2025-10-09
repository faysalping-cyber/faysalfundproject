using FaysalFundsWrapper.Models.AccountOpening;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class UploadDocumentationValidation : AbstractValidator<DocumentUpload>
    {
        public UploadDocumentationValidation()
        {
          RuleFor(x => x.CnicUploadFront)
    .NotNull().WithMessage("CNIC front image is required.")
    .Must(x => x.Length > 0).WithMessage("CNIC front image cannot be empty.");

RuleFor(x => x.CnicUploadBack)
    .NotNull().WithMessage("CNIC back image is required.")
    .Must(x => x.Length > 0).WithMessage("CNIC back image cannot be empty.");

RuleFor(x => x.LivePhotoOrSelfie)
    .NotNull().WithMessage("Live photo/selfie is required.")
    .Must(x => x.Length > 0).WithMessage("Live photo/selfie cannot be empty.");

        }
     
    }
}
