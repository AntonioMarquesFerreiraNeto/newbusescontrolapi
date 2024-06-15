using BusesControl.Entities.Request;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class EmployeeToggleTypeRequestValidator : AbstractValidator<EmployeeToggleTypeRequest>
{
    public EmployeeToggleTypeRequestValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Tipo é obrigatório")
            .IsInEnum().WithMessage("Tipo inválido");
    }
}
