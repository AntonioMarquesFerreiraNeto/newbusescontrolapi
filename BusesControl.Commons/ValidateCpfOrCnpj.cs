using System.Text.RegularExpressions;

namespace BusesControl.Commons;

public static class ValidateCpfOrCnpj
{
    public static bool CpfIsValid(string? cpf)
    {
        if (cpf is null)
        {
            return false;
        }

        cpf = OnlyNumbers.ClearValue(cpf);

        if (cpf.Length != 11)
        {
            return false;
        }

        if (new string(cpf[0], 11) == cpf)
        {
            return false;
        }

        int[] multiplicadores1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];
        }

        int remainder = sum % 11;
        if (remainder < 2)
        {
            remainder = 0;
        }
        else
        {
            remainder = 11 - remainder;
        }

        string digit = remainder.ToString();
        tempCpf = tempCpf + digit;
        sum = 0;

        for (int i = 0; i < 10; i++)
        {
            sum += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];
        }

        remainder = sum % 11;
        if (remainder < 2)
        {
            remainder = 0;
        }
        else
        {
            remainder = 11 - remainder;
        }

        digit = digit + remainder.ToString();

        return cpf.EndsWith(digit);
    }

    public static bool CnpjIsValid(string? cnpj)
    {
        if (string.IsNullOrEmpty(cnpj))
        {
            return false;
        }

        cnpj = Regex.Replace(cnpj, "[^0-9]", "");

        if (cnpj.Length != 14)
        {
            return false;
        }

        int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCnpj = cnpj.Substring(0, 12);
        int soma = 0;

        for (int i = 0; i < 12; i++)
        {
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
        }

        int resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        string digito = resto.ToString();
        tempCnpj = tempCnpj + digito;

        soma = 0;
        for (int i = 0; i < 13; i++)
        {
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
        }

        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito = digito + resto.ToString();

        return cnpj.EndsWith(digito);
    }

}
