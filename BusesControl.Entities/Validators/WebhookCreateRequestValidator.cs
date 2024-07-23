using BusesControl.Commons;
using BusesControl.Entities.Requests;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class WebhookCreateRequestValidator : AbstractValidator<WebhookCreateRequest>
{
    public WebhookCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é um campo obrigatório")
            .MinimumLength(10).WithMessage("Nome deve ter no mínimo 10 caracteres")
            .MaximumLength(70).WithMessage("Nome deve ter no máximo 70 caracteres");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url do webhook inválida")
            .MinimumLength(20).WithMessage("Url deve ter no mínimo 20 caracteres")
            .Matches(RegexString.UrlPattern).WithMessage("Url não atende aos padrões!");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é um campo obrigatório")
            .EmailAddress().WithMessage("E-mail inválido")
            .MaximumLength(80).WithMessage("E-mail deve ter no máximo 80 caracteres!");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Tipo é um campo obrigatório")
            .IsInEnum().WithMessage("Tipo é inválido");

        RuleFor(x => x.Events)
            .NotEmpty().WithMessage("Eventos do webhook não pode estar vazio");
    }
}
