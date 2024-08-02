using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class UserResetPasswordStepCodeRequestValidator : AbstractValidator<UserResetPasswordStepCodeRequest>
{
    public UserResetPasswordStepCodeRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é um campo obrigatório")
            .EmailAddress().WithMessage("Email inválido");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("Cpf é um campo obrigatório");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Data de nascimento é um campo obrigatório");
    }
}
