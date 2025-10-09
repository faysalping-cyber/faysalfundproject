//using FaysalFundsWrapper.Models;
//using FluentValidation;

//namespace FaysalFundsWrapper.Validations
//{
//    public class BankDetailsValidation : AbstractValidator<BankDetails>
//    {
//        private readonly string _wallet  = "Wallet";
//        private readonly string _bank  = "Bank";
//        private readonly string _reInvestment  = "Re-Investment";
//        private readonly string _encashment  = "Encashment";
//        public BankDetailsValidation()
//        {
//            RuleFor(x => x.TypeOfAccount).Must((typeOfAccount)=>typeOfAccount == _wallet || typeOfAccount== _bank).WithMessage($"Type of Account should be either {_wallet} or {_bank}");
//            RuleFor(x => x.Bank).NotEmpty().When(x => x.TypeOfAccount == _bank).WithMessage($"{_bank} must not be null or empty when TypeOfAccount is {_bank}");
//            RuleFor(x => x.IBAN).Must((model, iban) => model.TypeOfAccount != _bank || iban?.Length == 24).WithMessage($"IBAN must be 24 characters long when TypeOfAccount is {_bank}.");
//            RuleFor(x => x.Wallet).NotEmpty().When(x => x.TypeOfAccount == _wallet).WithMessage($"{_wallet} must not be null or empty when TypeOfAccount is {_wallet}");
//            RuleFor(x => x.WalletNumber).Must((model, number) => model.TypeOfAccount != _wallet || number?.Length == 11).WithMessage($"Wallet Number must be 11 digits long when TypeOfAccount is {_wallet}.");
//            RuleFor(x => x.DividendPayout).Must((dividendPayout) => dividendPayout == _reInvestment || dividendPayout == _encashment).WithMessage($"Dividend Payout should be either {_reInvestment} or {_encashment}");
//        }
//    }
//}


using FaysalFundsWrapper.Models;
using FluentValidation;
using System.Linq;

namespace FaysalFundsWrapper.Validations
{
    public class BankDetailsValidation : AbstractValidator<BankDetails>
    {
        private readonly string _wallet = "wallet";
        private readonly string _bank = "bank";
        private readonly string _reInvestment = "Re-Investment";
        private readonly string _encashment = "Encashment";


        public BankDetailsValidation()
        {
            // Type of Account must be either "wallet" or "bank"
            RuleFor(x => x.TypeOfAccount)
                .NotEmpty()
                .Must(x => x.ToLower() == _wallet || x.ToLower() == _bank)
                .WithMessage($"Type of Account should be either '{_wallet}' or '{_bank}'");

            // Bank-specific rules
            When(x => x.TypeOfAccount?.ToLower() == _bank, () =>
            {
                RuleFor(x => x.Bank)
                    .GreaterThan(0).WithMessage("Bank is required when TypeOfAccount is 'bank'.");

                RuleFor(x => x.IBAN)
                    .NotEmpty().WithMessage("IBAN is required when TypeOfAccount is 'bank'.")
                    .Length(24).WithMessage("IBAN must be exactly 24 characters.");

                RuleFor(x => x.Wallet)
                    .Equal(0).WithMessage("Wallet must be 0 when TypeOfAccount is 'bank'.");

                RuleFor(x => x.WalletNumber)
                    .Must(w => string.IsNullOrWhiteSpace(w))
                    .WithMessage("Wallet Number must be empty when TypeOfAccount is 'bank'.");
            });

            // Wallet-specific rules
            When(x => x.TypeOfAccount?.ToLower() == _wallet, () =>
            {
                RuleFor(x => x.Wallet)
                    .Must(w => w == 1 || w == 2)
                    .WithMessage("Wallet must be either Easypaisa or Jazzcash (1 or 2).");

                RuleFor(x => x.WalletNumber)
                    .NotEmpty().WithMessage("Wallet Number is required when TypeOfAccount is 'wallet'.")
                    .Length(11).WithMessage("Wallet Number must be exactly 11 digits.")
                    .Matches("^[0-9]{11}$").WithMessage("Wallet Number must be numeric.");

                RuleFor(x => x.Bank)
                    .Equal(0).WithMessage("Bank must be 0 when TypeOfAccount is 'wallet'.");

                RuleFor(x => x.IBAN)
                    .Must(w => string.IsNullOrWhiteSpace(w))
                    .WithMessage("IBAN Number must be empty when TypeOfAccount is 'Wallet'.");
            });

            // Dividend Payout rule
            RuleFor(x => x.DividendPayout)
                .NotEmpty()
                .Must(x => x == _reInvestment || x == _encashment)
                .WithMessage($"Dividend Payout should be either '{_reInvestment}' or '{_encashment}'");
        }
    }
}
