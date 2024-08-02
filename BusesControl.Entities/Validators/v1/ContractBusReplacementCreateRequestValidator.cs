using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class ContractBusReplacementCreateRequestValidator : AbstractValidator<ContractBusReplacementCreateRequest>
{
    public ContractBusReplacementCreateRequestValidator()
    {
        RuleFor(x => x.BusId)
            .NotEmpty().WithMessage("Ônibus é um campo obrigatório");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Data de início é um campo obrigatório");

        RuleFor(x => x.TerminateDate)
            .NotEmpty().WithMessage("Data de término é um campo obrigatório");

        RuleFor(x => x.ReasonDescription)
            .NotEmpty().WithMessage("Descrição do motivo é um campo obrigatório")
            .MinimumLength(10).WithMessage("Descrição do motivo deve ter no mínimo 10 caracteres")
            .MaximumLength(500).WithMessage("Descrição do motivo deve ter no máximo 500 caracteres");
    }
}
