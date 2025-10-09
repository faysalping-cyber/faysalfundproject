using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class CrsValidation : AbstractValidator<CRS>
    {

        private const int PakistanCountryId = 1;

        public CrsValidation()
        {
            RuleFor(e => e.TaxResidentCountry)
                .NotEmpty().WithMessage("Tax Resident Country is required.")
                .NotNull();

            RuleFor(e => e.CRS_Declaration)
                .NotNull().WithMessage("CRS Declaration is required.")
                .Equal(1).WithMessage("CRS Declaration must be required.");
            RuleFor(e => e.TIN_Number)
        .MaximumLength(25)
        .WithMessage("cannot be greater than 25 characters.");



            // --- If country is NOT Pakistan ---
            //When(e => e.TaxResidentCountry != PakistanCountryId, () =>
            //{

            //    RuleFor(e => e.TIN_Number)
            //        .NotEmpty().WithMessage("TIN Number is required.")
            //        .Length(4).WithMessage("TIN Number must be 4 characters.");

            //    RuleFor(e => e.ReasonForNoTIN)
            //    .NotNull().WithMessage("Reason for no TIN is required.")
            //    .Equal(1);


            //    RuleFor(e => e.HaveTIN)
            //   .NotNull().WithMessage("Have TIN selection is not required.")
            //   .Equal(0);
            //});

            // --- If country is Pakistan ---
            // When(e => e.TaxResidentCountry == PakistanCountryId, () =>
            // {
            //     RuleFor(e => e.TIN_Number)
            //         .Empty().WithMessage("TIN Number should not be provided for Pakistan.");

            //     RuleFor(e => e.ReasonForNoTIN)
            //.NotNull().WithMessage("Reason for no TIN is not required.")
            //.Equal(0);

            //     RuleFor(e => e.HaveTIN)
            //  .NotNull().WithMessage("Have TIN selection is required.")
            //  .Equal(1);
            // });
        }
    }

}
