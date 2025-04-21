using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1
{
    public class ContactCreateRequestValidator : AbstractValidator<ContactCreateRequest>
    {
        public ContactCreateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome obrigatório!")
                .MaximumLength(150).WithMessage("Nome deve ter no máximo 150 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail obrigatório!")
                .MaximumLength(150).WithMessage("E-mail deve ter no máximo 150 caracteres");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefone obrigatório!")
                .MaximumLength(150).WithMessage("Telefone deve ter no máximo 150 caracteres");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Mensagem obrigatório!")
                .MaximumLength(2000).WithMessage("Mensagem deve ter no máximo 2000 caracteres");
        }
    }
}
