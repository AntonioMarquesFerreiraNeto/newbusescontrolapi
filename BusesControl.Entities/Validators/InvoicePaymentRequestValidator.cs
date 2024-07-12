using BusesControl.Commons;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Requests;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class InvoicePaymentRequestValidator : AbstractValidator<InvoicePaymentRequest>
{
    public InvoicePaymentRequestValidator()
    {
        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Método de pagamento é um campo obrigatório")
            .IsInEnum().WithMessage("Método de pagamento inválido");

        When(x => x.PaymentMethod == PaymentMethodEnum.CreditCard && x.CreditCard is not null, () =>
        {
            RuleFor(x => x.CreditCard!.HolderName)
                .NotEmpty().WithMessage("Nome do titular do cartão é obrigatório");

            RuleFor(x => x.CreditCard!.HolderCpfCnpj)
                .NotEmpty().WithMessage("CPF ou CNPJ do titular do cartão é obrigatório");

            RuleFor(x => x.CreditCard!.HolderEmail)
                .NotEmpty().WithMessage("E-mail do titular do cartão é obrigatório");

            RuleFor(x => x.CreditCard!.HolderMobilePhone)
                .NotEmpty().WithMessage("Telefone celular do titular do cartão é obrigatório");

            RuleFor(x => x.CreditCard!.HolderPostalCode)
                .NotEmpty().WithMessage("CEP do titular do cartão é obrigatório");

            RuleFor(x => x.CreditCard!.HolderAddressNumber)
                .NotEmpty().WithMessage("Número do endereço do titular do cartão é obrigatório");

            RuleFor(x => x.CreditCard!.Number)
                .NotEmpty().WithMessage("Número do cartão é obrigatório")
                .Must(y => OnlyNumbers.ClearValue(y).Length == 16).WithMessage("Número do cartão inválido");

            RuleFor(x => x.CreditCard!.ExpiryMonth)
                .NotEmpty().WithMessage("Mês de validade do cartão é obrigatório");

            RuleFor(x => x.CreditCard!.ExpiryYear)
                .NotEmpty().WithMessage("Ano de validade do cartão é obrigatório");

            RuleFor(x => x.CreditCard!.SecurityCode)
                .NotEmpty().WithMessage("Código de segurança do cartão é obrigatório");
        });
    }
}
