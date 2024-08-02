using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class UserResetPasswordStepResetTokenRequestValidator : AbstractValidator<UserResetPasswordStepResetTokenRequest>
{
    public UserResetPasswordStepResetTokenRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Código é um campo obrigatório");
    }
}
