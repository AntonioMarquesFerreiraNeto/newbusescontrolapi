using BusesControl.Entities.Request;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("E-mail é um campo obrigatório!");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é um campo obrigatório!");
    }
}
