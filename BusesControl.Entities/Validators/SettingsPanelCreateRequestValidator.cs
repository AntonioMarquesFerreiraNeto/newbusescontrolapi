using BusesControl.Entities.Requests;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class SettingsPanelCreateRequestValidator : AbstractValidator<SettingsPanelCreateRequest>
{
    public SettingsPanelCreateRequestValidator()
    {
        RuleFor(x => x.Parent)
            .NotEmpty().WithMessage("Parent é um campo obrigatório!")
            .IsInEnum().WithMessage("Parent inválido!");

        RuleFor(x => x.TerminationFee)
            .LessThanOrEqualTo(30).WithMessage("Taxa de rescisão não pode ultrapassar 30%");

        RuleFor(x => x.LimitDateTermination)
            .GreaterThanOrEqualTo(1).WithMessage("Limite mínimo de anos de contratos tem que maior ou igual a 1")
            .When(x => x.LimitDateTermination is not null);

        RuleFor(x => x.LateFeeInterestRate)
            .LessThanOrEqualTo(30).WithMessage("Taxa de juros não pode ultrapassar 30%");
    }
}
