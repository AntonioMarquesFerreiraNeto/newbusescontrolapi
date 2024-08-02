using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class UserToggleActiveRequestValidator : AbstractValidator<UserToggleActiveRequest>
{
    public UserToggleActiveRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status é um campo obrigatório")
            .IsInEnum().WithMessage("Status inválido");
    }
}
