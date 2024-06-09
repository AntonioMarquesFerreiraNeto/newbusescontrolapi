using BusesControl.Entities.Request;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class UserToggleActiveRequestValidator : AbstractValidator<UserToggleActiveRequest>
{
    public UserToggleActiveRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status é um campo obrigatório!")
            .IsInEnum().WithMessage("Status inválido!");
    }
}
