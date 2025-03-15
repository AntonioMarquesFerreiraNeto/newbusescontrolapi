using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1
{
    public class GenerativePostRequestValidator : AbstractValidator<GenerativePostRequest>
    {
        public GenerativePostRequestValidator() 
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("A pergunta é um campo obrigatório");
        }
    }
}
