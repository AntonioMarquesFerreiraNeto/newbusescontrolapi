using BusesControl.Entities.Requests;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class SettingsPanelUpdateRequestValidator : AbstractValidator<SettingsPanelUpdateRequest>
{
    public SettingsPanelUpdateRequestValidator()
    {
        RuleFor(x => x.Parent)
            .NotEmpty().WithMessage("Parent é um campo obrigatório!")
            .IsInEnum().WithMessage("Parent inválido!");

        RuleFor(x => x.TerminationFee)
            .LessThanOrEqualTo(30).WithMessage("Taxa de rescisão não pode ultrapassar 30%");

        RuleFor(x => x.LateFeeInterestRate)
            .LessThanOrEqualTo(30).WithMessage("Taxa de juros não pode ultrapassar 30%");
    }
}
