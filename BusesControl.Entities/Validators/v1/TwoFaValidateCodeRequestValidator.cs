using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1
{
    public class TwoFaValidateCodeRequestValidator : AbstractValidator<TwoFaValidateCodeRequest>
    {
        public TwoFaValidateCodeRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é um campo obrigatório!");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Código é um campo obrigatório!");
        }
    }
}
