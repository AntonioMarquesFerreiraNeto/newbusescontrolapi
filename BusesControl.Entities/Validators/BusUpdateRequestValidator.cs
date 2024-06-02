using BusesControl.Entities.Request;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class BusUpdateRequestValidator : AbstractValidator<BusUpdateRequest>
{
    public BusUpdateRequestValidator()
    {
        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Marca é um campo obrigatório.")
            .MinimumLength(3).WithMessage("Marca deve ter no mínimo 3 caracteres.")
            .MaximumLength(60).WithMessage("Marca deve ter no máximo 60 caracteres."); ;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é um campo obrigatório.")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(60).WithMessage("Nome deve ter no máximo 60 caracteres.");

        RuleFor(x => x.ManufactureDate)
            .NotEmpty().WithMessage("Fabricação é um campo obrigatório.");

        RuleFor(x => x.Renavam)
            .NotEmpty().WithMessage("Renavam é um campo obrigatório.")
            .Length(11).WithMessage("Renavam deve ter 3 caracteres.");

        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("Placa é um campo obrigatório.")
            .Length(8).WithMessage("Placa deve ter 8 caracteres.");

        RuleFor(x => x.Chassi)
            .NotEmpty().WithMessage("Chassi é um campo obrigatório.")
            .Length(17).WithMessage("Chassi deve ter 17 caracteres.");

        RuleFor(x => x.SeatingCapacity)
            .NotEmpty().WithMessage("Capacidade de assentos é um campo obrigatório.")
            .LessThanOrEqualTo(240).WithMessage("Capacidade não permitida.");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Cor é um campo obrigatório.")
            .MinimumLength(3).WithMessage("Marca deve ter no mínimo 3 caracteres.")
            .MaximumLength(30).WithMessage("Cor deve ter no máximo 30 caracteres.");
    }
}
