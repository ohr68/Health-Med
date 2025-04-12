using System.Text.RegularExpressions;
using HealthMed.Domain.Exceptions;

namespace HealthMed.Domain.ValueObjects;

public class Cpf
{
    private static readonly Regex CpfRegex = new(@"^\d{11}$", RegexOptions.Compiled);

    public string Valor { get; protected set; }

    public Cpf(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException("O CPF não pode ser vazio.");

        valor = new string(valor.Where(char.IsDigit).ToArray());

        if (!CpfRegex.IsMatch(valor))
            throw new DomainException("O CPF informado é inválido.");

        if (!CpfValido(valor))
            throw new DomainException("O CPF informado não é válido.");

        Valor = valor;
    }

    public override string ToString() =>
        Convert.ToUInt64(Valor).ToString(@"000\.000\.000\-00");

    public static implicit operator string(Cpf cpf) => cpf.Valor;

    public static implicit operator Cpf(string valor) => new Cpf(valor);

    public override bool Equals(object obj)
    {
        if (obj is Cpf other)
            return string.Equals(Valor, other.Valor);

        return false;
    }

    public override int GetHashCode()
    {
        return Valor.GetHashCode();
    }

    public static bool TryParse(string valor, out Cpf? cpf)
    {
        if (EhValido(valor))
        {
            cpf = new Cpf(valor);
            return true;
        }

        cpf = null;
        return false;
    }
    
    public static bool TryParse(string valor) => EhValido(valor);
    
    private static bool EhValido(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return false;

        valor = valor.Trim();
        return CpfRegex.IsMatch(valor);
    }
    
    private static bool CpfValido(string cpf)
    {
        if (cpf.All(c => c == cpf[0])) return false;

        int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCpf = cpf.Substring(0, 9);
        var soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];

        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        tempCpf += digito1;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];

        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith($"{digito1}{digito2}");
    }
}