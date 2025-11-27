using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1
{
    public class CreateTwoRequestValidator : AbstractValidator<CreateTwoRequest>
    {
        public CreateTwoRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é um campo obrigatório!");
        }
    }
}
