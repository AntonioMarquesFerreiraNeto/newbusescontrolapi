using BusesControl.Entities.Request;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class UserRegistrationStepTokenRequestValidator : AbstractValidator<UserRegistrationStepTokenRequest>
{
    public UserRegistrationStepTokenRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Código é um campo obrigatório");
    }
}
