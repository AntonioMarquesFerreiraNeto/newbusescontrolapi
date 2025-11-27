using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1
{
    public class CreateTwoFaForNewRequestValidator : AbstractValidator<TwoFaCheckForNewRequest>
    {
        public CreateTwoFaForNewRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é um campo obrigatório!");
        }
    }
}
