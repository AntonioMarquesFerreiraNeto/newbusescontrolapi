﻿using BusesControl.Entities.Requests;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class SupportTicketMessageCreateRequestValidator : AbstractValidator<SupportTicketMessageCreateRequest>
{
    public SupportTicketMessageCreateRequestValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Mensagem é um campo obrigatório")
            .MinimumLength(3).WithMessage("Mensagem deve ter no mínimo 3 caracteres")
            .MaximumLength(600).WithMessage("Mensagem deve ter no máximo 600 caracteres");
    }
}
