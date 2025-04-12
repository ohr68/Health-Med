using System.Text.RegularExpressions;
using HealthMed.Domain.Exceptions;

namespace HealthMed.Domain.ValueObjects;

public class Email
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Valor { get; protected set; }

    public Email(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException("O email não pode ser vazio.");

        if (!EmailRegex.IsMatch(valor))
            throw new DomainException("O email informado é inválido.");

        Valor = valor;
    }

    public override string ToString() => Valor;

    // Conversão implícita para string
    public static implicit operator string(Email email) => email.Valor;

    // Conversão implícita de string para Email
    public static implicit operator Email(string valor) => new Email(valor);
    
    public override bool Equals(object obj)
    {
        if (obj is Email otherEmail)
            return string.Equals(Valor, otherEmail.Valor, StringComparison.OrdinalIgnoreCase);

        return false;
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Valor);
    }
    
    public static bool TryParse(string valor, out Email? email)
    {
        if (EhValido(valor))
        {
            email = new Email(valor);
            return true;
        }

        email = null;
        return false;
    }
    
    public static bool TryParse(string valor) => EhValido(valor);
    
    
    private static bool EhValido(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) return false;
        valor = valor.Trim().ToLowerInvariant();
        return EmailRegex.IsMatch(valor);
    }

}