using System.Text.RegularExpressions;
using HealthMed.Domain.Exceptions;

namespace HealthMed.Domain.ValueObjects;

public class Crm
{
    private static readonly Regex CrmRegex = new(@"^\d{4,7}/[A-Z]{2}$", RegexOptions.Compiled);

    public string Valor { get; protected set; }

    public Crm(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException("O CRM não pode ser vazio.");

        valor = valor.ToUpper().Trim();

        if (!CrmRegex.IsMatch(valor))
            throw new DomainException("O CRM informado é inválido. Use o formato 123456/SP.");

        Valor = valor;
    }

    public override string ToString() => Valor;

    public static implicit operator string(Crm crm) => crm.Valor;

    public static implicit operator Crm(string valor) => new(valor);

    public override bool Equals(object? obj)
    {
        if (obj is Crm otherCrm)
            return string.Equals(Valor, otherCrm.Valor, StringComparison.OrdinalIgnoreCase);

        return false;
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Valor);
    }
    
    public static bool TryParse(string valor, out Crm? crm)
    {
        if (EhValido(valor))
        {
            crm = new Crm(valor);
            return true;
        }

        crm = null;
        return false;
    }
    
    private static bool EhValido(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return false;

        valor = valor.ToUpper().Trim();
        return CrmRegex.IsMatch(valor);
    }
    
}