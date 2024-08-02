using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class UserRegistrationStepTokenRequestValidator : AbstractValidator<UserRegistrationStepTokenRequest>
{
    public UserRegistrationStepTokenRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Código é um campo obrigatório");
    }
}
