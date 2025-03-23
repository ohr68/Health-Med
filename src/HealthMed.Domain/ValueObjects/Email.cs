using System.Text.RegularExpressions;
using HealthMed.Domain.Exceptions;

public sealed record Email
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Valor { get; }

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
}