using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class NotificationCreateRequestValidator : AbstractValidator<NotificationCreateRequest>
{
    public NotificationCreateRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Título é um campo obrigatório")
            .MinimumLength(3).WithMessage("Título deve ter no mínimo 3 caracteres")
            .MaximumLength(25).WithMessage("Títul deve ter no máximo 25 caracteres");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Mensagem é um campo obrigatório")
            .MinimumLength(10).WithMessage("Mensagem deve ter no mínimo 10 caracteres")
            .MaximumLength(200).WithMessage("Mensagem deve ter no máximo 200 caracteres");

        RuleFor(x => x.AccessLevel)
            .NotEmpty().WithMessage("Nível de acesso é um campo obrigatório")
            .IsInEnum().WithMessage("Nível de acesso inválido");
    }
}
