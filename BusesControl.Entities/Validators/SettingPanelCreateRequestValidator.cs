using BusesControl.Entities.Requests;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class SettingPanelCreateRequestValidator : AbstractValidator<SettingPanelCreateRequest>
{
    public SettingPanelCreateRequestValidator()
    {
        RuleFor(x => x.Parent)
            .NotEmpty().WithMessage("Parent é um campo obrigatório")
            .IsInEnum().WithMessage("Parent inválido");

        RuleFor(x => x.TerminationFee)
            .NotEmpty().WithMessage("Taxa de rescisão é um campo obrigatório")
            .GreaterThanOrEqualTo(1).WithMessage("Taxa de rescisão não pode ser menor que 1%")
            .LessThanOrEqualTo(10).WithMessage("Taxa de rescisão não pode ultrapassar 10%");

        RuleFor(x => x.LimitDateTerminate)
            .GreaterThanOrEqualTo(1).WithMessage("Limite mínimo de anos de contratos tem que maior ou igual a 1")
            .When(x => x.LimitDateTerminate is not null);

        RuleFor(x => x.LateFeeInterestRate)
            .GreaterThanOrEqualTo(1).WithMessage("Taxa de juros não pode ser menor que 1%")
            .LessThanOrEqualTo(10).WithMessage("Taxa de juros não pode ultrapassar 10%");
    }
}
