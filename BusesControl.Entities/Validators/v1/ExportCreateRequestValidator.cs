using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1
{
    public class ExportCreateRequestValidator : AbstractValidator<ExportCreateRequest>
    {
        public ExportCreateRequestValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Tipo é um campo obrigatório")
                .IsInEnum().WithMessage("Tipo inválido");

            RuleFor(x => x.DocumentType)
                .NotEmpty().WithMessage("Tipo do documento é obrigatório")
                .IsInEnum().WithMessage("Tipo do documento inválido");
        }
    }
}
