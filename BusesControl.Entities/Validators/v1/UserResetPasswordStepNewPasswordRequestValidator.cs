﻿using BusesControl.Commons;
using BusesControl.Entities.Requests.v1;
using FluentValidation;

namespace BusesControl.Entities.Validators.v1;

public class UserResetPasswordStepNewPasswordRequestValidator : AbstractValidator<UserResetPasswordStepNewPasswordRequest>
{
    public UserResetPasswordStepNewPasswordRequestValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Nova senha é um campo obrigatório")
            .MinimumLength(10).WithMessage("Nova senha deve ter no minimo 10 caracteres")
            .Matches(RegexString.PasswordPattern).WithMessage("A senha deve conter pelo menos um dígito, uma letra minúscula e um caractere especial");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirmar senha é um campo obrigatório")
            .MinimumLength(10).WithMessage("Confirmar senha deve ter no minimo 10 caracteres")
            .Matches(RegexString.PasswordPattern).WithMessage("A senha deve conter pelo menos um dígito, uma letra minúscula e um caractere especial");
    }
}
