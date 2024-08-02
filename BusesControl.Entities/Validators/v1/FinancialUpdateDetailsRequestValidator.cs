using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class FinancialUpdateDetailsRequestValidator : AbstractValidator<FinancialUpdateDetailsRequest>
{
    public FinancialUpdateDetailsRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Título é um campo obrigatório")
            .MinimumLength(10).WithMessage("Título deve ter no mínimo 10 caracteres")
            .MaximumLength(120).WithMessage("Título deve ter no máximo 120 caracteres");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição é um campo obrigatório")
            .MinimumLength(20).WithMessage("Descrição deve ter no mínimo 20 caracteres")
            .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres");
    }
}
