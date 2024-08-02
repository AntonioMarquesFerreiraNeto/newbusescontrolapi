using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class ContractDescriptionCreateRequestValidator : AbstractValidator<ContractDescriptionCreateRequest>
{
    public ContractDescriptionCreateRequestValidator()
    {
        RuleFor(x => x.Owner)
            .NotEmpty().WithMessage("Empresa contratada é um campo obrigatório")
            .MinimumLength(200).WithMessage("Empresa contratada deve ter no mínimo 200 caracteres")
            .MaximumLength(3000).WithMessage("Empresa contratada deve ter no máximo 3000 caracteres");

        RuleFor(x => x.GeneralProvisions)
            .NotEmpty().WithMessage("Disposições gerais é um campo obrigatório")
            .MinimumLength(200).WithMessage("Disposições gerais deve ter no mínimo 200 caracteres")
            .MaximumLength(3000).WithMessage("Disposições gerais deve ter no máximo 3000 caracteres");

        RuleFor(x => x.Objective)
            .NotEmpty().WithMessage("Objetivo é um campo obrigatório")
            .MinimumLength(200).WithMessage("Objetivo deve ter no mínimo 200 caracteres")
            .MaximumLength(3000).WithMessage("Objetivo deve ter no máximo 3000 caracteres");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Título é um campo obrigatório")
            .MinimumLength(10).WithMessage("Título deve ter no mínimo 10 caracteres")
            .MaximumLength(70).WithMessage("Título deve ter no máximo 3000 caracteres");

        RuleFor(x => x.SubTitle)
            .NotEmpty().WithMessage("Subtítulo é um campo obrigatório")
            .MinimumLength(10).WithMessage("Subtítulo deve ter no mínimo 10 caracteres")
            .MaximumLength(70).WithMessage("Subtítulo deve ter no máximo 3000 caracteres");

        RuleFor(x => x.Copyright)
            .NotEmpty().WithMessage("Copyright é um campo obrigatório")
            .MinimumLength(10).WithMessage("Copyright deve ter no mínimo 10 caracteres")
            .MaximumLength(70).WithMessage("Copyright deve ter no máximo 3000 caracteres");
    }
}
