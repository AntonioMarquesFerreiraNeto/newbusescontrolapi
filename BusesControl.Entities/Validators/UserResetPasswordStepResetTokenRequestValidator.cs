using BusesControl.Entities.Request;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class UserResetPasswordStepResetTokenRequestValidator : AbstractValidator<UserResetPasswordStepResetTokenRequest>
{
    public UserResetPasswordStepResetTokenRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Código é um campo obrigatório");
    }
}
