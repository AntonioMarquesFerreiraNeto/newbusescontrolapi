

using System.Text.RegularExpressions;

namespace BusesControl.Commons;

public static class ValidateCpf
{
    public static bool IsValid(string cpf)
    {
        cpf = Regex.Replace(cpf, @"[.\-]", string.Empty);

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
}
