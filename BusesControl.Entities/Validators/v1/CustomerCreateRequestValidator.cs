using BusesControl.Commons;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class CustomerCreateRequestValidator : AbstractValidator<CustomerCreateRequest>
{
    public CustomerCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é um campo obrigatório")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres")
            .MaximumLength(60).WithMessage("Nome deve ter no mínimo 60 caracteres");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("Cpf é um campo obrigatório")
            .Must(ValidateCpfOrCnpj.CpfIsValid).WithMessage("Cpf inválido")
            .When(x => x.Type == CustomerTypeEnum.NaturalPerson);

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage("Cnpj é um campo obrigatório")
                .Must(ValidateCpfOrCnpj.CnpjIsValid).WithMessage("Cnpj inválido")
                .When(x => x.Type == CustomerTypeEnum.LegalEntity);

        When(x => x.Type == CustomerTypeEnum.NaturalPerson, () =>
        {
            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Data de nascimento é um campo obrigatório");
        });

        When(x => x.Type == CustomerTypeEnum.LegalEntity, () =>
        {
            RuleFor(x => x.OpenDate)
                .NotEmpty().WithMessage("Data de abertura é um campo obrigatório");
        });

        When(x => x.BirthDate is not null, () =>
        {
            RuleFor(x => x.BirthDate!.Value)
                .Must(ValidateBirthDateOfLegalAge.IsValid).WithMessage("Data de nascimento inválida");
        });

        When(x => x.OpenDate is not null, () =>
        {
            RuleFor(x => x.OpenDate!.Value)
                .Must(ValidateOpenDate.IsValid).WithMessage("Data de abertura inválida");
        });

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é um campo obrigatório")
            .EmailAddress().WithMessage("E-mail inválido")
            .MaximumLength(80).WithMessage("E-mail deve ter no máximo 80 caracteres");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Número de telefone é um campo obrigatório")
            .Must(ValidatePhoneNumber.IsValid).WithMessage("Número de telefone inválido");

        RuleFor(x => x.HomeNumber)
            .NotEmpty().WithMessage("Número residencial é um campo obrigatório")
            .MaximumLength(20).WithMessage("Número residencial deve ter no mínimo 20 caracteres");

        RuleFor(x => x.Logradouro)
            .NotEmpty().WithMessage("Logradouro é um campo obrigatório")
            .MaximumLength(60).WithMessage("Logradouro deve ter no mínimo 60 caracteres");

        RuleFor(x => x.ComplementResidential)
            .NotEmpty().WithMessage("Complemento residencial é um campo obrigatório")
            .MaximumLength(80).WithMessage("Complemento residencial deve ter no mínimo 60 caracteres");

        RuleFor(x => x.Neighborhood)
            .NotEmpty().WithMessage("Bairro é um campo obrigatório")
            .MaximumLength(80).WithMessage("Bairro deve ter no mínimo 60 caracteres");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Cidade é um campo obrigatório")
            .MaximumLength(80).WithMessage("Cidade deve ter no mínimo 60 caracteres");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("UF é um campo obrigatório")
            .Length(2).WithMessage("UF deve ter dois caracteres");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Tipo é um campo obrigatório")
            .IsInEnum().WithMessage("Tipo inválido");

        When(x => x.Type == CustomerTypeEnum.NaturalPerson, () =>
        {
            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Sexo é um campo obrigatório")
                .IsInEnum().WithMessage("Sexo inválido");
        });
    }
}
