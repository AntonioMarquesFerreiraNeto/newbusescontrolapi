using BusesControl.Entities.Enums;
using BusesControl.Entities.Requests;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class InvoiceExpensePaymentRequestValidator : AbstractValidator<InvoiceExpensePaymentRequest>
{
    public InvoiceExpensePaymentRequestValidator()
    {
        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Método de pagamento é um campo obrigatório")
            .IsInEnum().WithMessage("Método de pagamento inválido");

        RuleFor(x => x.PixRequest)
            .NotEmpty().WithMessage("Deve ser informado os dados pagamento via PIX").When(x => x.PaymentMethod == PaymentExpenseMethodEnum.Pix);

        When(x => x.PaymentMethod == PaymentExpenseMethodEnum.Pix && x.PixRequest is not null, () => 
        {
            RuleFor(x => x.PixRequest!.BankCode)
                .NotEmpty().WithMessage("Código é um campo obrigatório");

            RuleFor(x => x.PixRequest!.AccountName)
                .NotEmpty().WithMessage("Nome da conta é um campo obrigatório");

            RuleFor(x => x.PixRequest!.OwnerName)
                .NotEmpty().WithMessage("Nome do proprietário da conta é um campo obrigatório");

            RuleFor(x => x.PixRequest!.OwnerBirthDate)
                .NotEmpty().WithMessage("Data de nascimento do proprietário da conta é um campo obrigatório");

            RuleFor(x => x.PixRequest!.CpfCnpj)
                .NotEmpty().WithMessage("CPF ou CNPJ do proprietário da conta bancária é um campo obrigatório");

            RuleFor(x => x.PixRequest!.Agency)
                .NotEmpty().WithMessage("Número da agência é um campo obrigatório");

            RuleFor(x => x.PixRequest!.Account)
                .NotEmpty().WithMessage("Número da conta bancária é um campo obrigatório");

            RuleFor(x => x.PixRequest!.AccountDigit)
                .NotEmpty().WithMessage("Dígito da conta bancária é um campo obrigatório");
        });
    }
}
