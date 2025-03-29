using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class SupportTicketMessageCreateRequestValidator : AbstractValidator<SupportTicketMessageCreateRequest>
{
    public SupportTicketMessageCreateRequestValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Mensagem é um campo obrigatório")
            .MaximumLength(600).WithMessage("Mensagem deve ter no máximo 600 caracteres");
    }
}
