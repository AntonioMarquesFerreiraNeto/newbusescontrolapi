using BusesControl.Entities.Request;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class UserSetNickNameRequestValidator : AbstractValidator<UserSetNickNameRequest>
{
    public UserSetNickNameRequestValidator()
    {
        RuleFor(x => x.Nickname)
            .NotEmpty().WithMessage("Nickname é um campo obrigatório")
            .MaximumLength(20).WithMessage("Nickname deve ter no máximo 20 caracteres");
    }
}
