using BusesControl.Commons;
using BusesControl.Entities.Request;
using FluentValidation;

namespace BusesControl.Entities.Validators;

public class EmployeeUpdateRequestValidator : AbstractValidator<EmployeeUpdateRequest>
{
    public EmployeeUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é um campo obrigatório")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres!")
            .MaximumLength(60).WithMessage("Nome deve ter no mínimo 60 caracteres!");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("Cpf é um campo obrigatório")
            .Must(ValidateCpfOrCnpj.CpfIsValid).WithMessage("Cpf inválido!");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Data de nascimento é um campo obrigatório")
            .Must(ValidateBirthDateOfLegalAge.IsValid).WithMessage("Data de nascimento inválida!");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é um campo obrigatório")
            .MaximumLength(80).WithMessage("E-mail deve ter no mínimo 80 caracteres!");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Número de telefone é um campo obrigatório")
            .Must(ValidatePhoneNumber.IsValid).WithMessage("Número de telefone inválido!");

        RuleFor(x => x.HomeNumber)
            .NotEmpty().WithMessage("Número residencial é um campo obrigatório")
            .MaximumLength(20).WithMessage("Número residencial deve ter no mínimo 20 caracteres!");

        RuleFor(x => x.Logradouro)
            .NotEmpty().WithMessage("Logradouro é um campo obrigatório")
            .MaximumLength(60).WithMessage("Logradouro deve ter no mínimo 60 caracteres!"); ;

        RuleFor(x => x.ComplementResidential)
            .NotEmpty().WithMessage("Complemento residencial é um campo obrigatório")
            .MaximumLength(80).WithMessage("Complemento residencial deve ter no mínimo 60 caracteres!"); ;

        RuleFor(x => x.Neighborhood)
            .NotEmpty().WithMessage("Bairro é um campo obrigatório")
            .MaximumLength(80).WithMessage("Bairro deve ter no mínimo 60 caracteres!"); ;

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Cidade é um campo obrigatório")
            .MaximumLength(80).WithMessage("Cidade deve ter no mínimo 60 caracteres!"); ;

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("UF é um campo obrigatório!")
            .Length(2).WithMessage("UF deve ter dois caracteres!");
    }
}
