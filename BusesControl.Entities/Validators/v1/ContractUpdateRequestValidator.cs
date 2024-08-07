﻿using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class ContractUpdateRequestValidator : AbstractValidator<ContractUpdateRequest>
{
    public ContractUpdateRequestValidator()
    {
        RuleFor(x => x.BusId)
            .NotEmpty().WithMessage("Ônibus é um campo obrigatório");

        RuleFor(x => x.DriverId)
            .NotEmpty().WithMessage("Motorista é um campo obrigatório");

        RuleFor(x => x.SettingPanelId)
            .NotEmpty().WithMessage("Painel de configurações é um campo obrigatório");

        RuleFor(x => x.ContractDescriptionId)
            .NotEmpty().WithMessage("Descrição do contrato é um campo obrigatório");

        RuleFor(x => x.PaymentType)
            .NotEmpty().WithMessage("Tipo de pagamento é um campo obrigatório");

        RuleFor(x => x.TotalPrice)
            .NotEmpty().WithMessage("Preço total é um campo obrigatório")
            .GreaterThanOrEqualTo(50).WithMessage("Preço total deve ter no mínimo R$ 50,00");

        RuleFor(x => x.Details)
            .NotEmpty().WithMessage("Detalhes é um campo obrigatório")
            .MinimumLength(20).WithMessage("Detalhes deve ter no mínimo 20 caracteres")
            .MaximumLength(2500).WithMessage("Detalhes deve ter no máximo 2500 caracteres");

        RuleFor(x => x.TerminateDate)
            .NotEmpty().WithMessage("Data de término é um campo obrigatório");

        RuleFor(x => x.CustomersId)
            .NotEmpty().WithMessage("A lista de clientes não pode estar vazia");
    }
}
