using BusesControl.Entities.Requests;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class SupportTicketCreateRequestValidator : AbstractValidator<SupportTicketCreateRequest>
{
    public SupportTicketCreateRequestValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Tipo de ticket é um campo obrigatório")
            .IsInEnum().WithMessage("Tipo de ticket inválido");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Título é um campo obrigatório")
            .MinimumLength(3).WithMessage("Título deve ter no mínimo 3 caracteres")
            .MaximumLength(40).WithMessage("Título deve ter no máximo 40 caracteres");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Mensagem é um campo obrigatório")
            .MinimumLength(3).WithMessage("Mensagem deve ter no mínimo 3 caracteres")
            .MaximumLength(600).WithMessage("Mensagem deve ter no máximo 600 caracteres");
    }
}
