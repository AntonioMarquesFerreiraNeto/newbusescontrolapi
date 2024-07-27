using BusesControl.Entities.Requests;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class FinancialExpenseCreateRequestValidator : AbstractValidator<FinancialExpenseCreateRequest>
{
    public FinancialExpenseCreateRequestValidator()
    {
        RuleFor(x => x.SupplierId)
            .NotEmpty().WithMessage("Fornecedor é um campo obrigatório");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Título é um campo obrigatório")
            .MinimumLength(10).WithMessage("Título deve ter no mínimo 10 caracteres")
            .MaximumLength(120).WithMessage("Título deve ter no máximo 120 caracteres");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição é um campo obrigatório")
            .MinimumLength(20).WithMessage("Descrição deve ter no mínimo 20 caracteres")
            .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres");

        RuleFor(x => x.TotalPrice)
            .NotEmpty().WithMessage("Preço total é um campo obrigatório")
            .GreaterThanOrEqualTo(50).WithMessage("Preço total deve ter no mínimo R$ 50,00");

        RuleFor(x => x.PaymentType)
            .NotEmpty().WithMessage("Tipo do pagamento é um campo obrigatório")
            .IsInEnum().WithMessage("Tipo de pagamento inválido");

        RuleFor(x => x.TerminateDate)
            .NotEmpty().WithMessage("Data de término é um campo obrigatório");
    }
}
