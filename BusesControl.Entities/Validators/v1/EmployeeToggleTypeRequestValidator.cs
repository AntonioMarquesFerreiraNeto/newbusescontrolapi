using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class EmployeeToggleTypeRequestValidator : AbstractValidator<EmployeeToggleTypeRequest>
{
    public EmployeeToggleTypeRequestValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Tipo é obrigatório")
            .IsInEnum().WithMessage("Tipo inválido");
    }
}
