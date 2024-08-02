using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("E-mail é um campo obrigatório");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é um campo obrigatório");
    }
}
