﻿using BusesControl.Entities.Requests;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class ColorCreateRequestValidator : AbstractValidator<ColorCreateRequest>
{
    public ColorCreateRequestValidator()
    {
        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Cor é um campo obrigatório")
            .MinimumLength(3).WithMessage("Cor deve ter no mínimo 3 caracteres")
            .MaximumLength(30).WithMessage("Cor deve ter no máximo 30 caracteres");
    }
}
