using HealthMed.Domain.Exceptions;

namespace HealthMed.Domain.ValueObjects;

public sealed record Hora
{
    public int Valor { get; }

    public Hora(int valor)
    {
        if (valor is < 0 or > 23)
            throw new DomainException("Hora inválida. Deve estar entre 0 e 23.");

        Valor = valor;
    }

    public override string ToString() => Valor.ToString("D2"); // Formata como "00", "01", ..., "23"

    public static Hora DeValor(int valor) => new(valor);

    public static implicit operator int(Hora hora) => hora.Valor;
    public static implicit operator Hora(int valor) => new(valor);
}
