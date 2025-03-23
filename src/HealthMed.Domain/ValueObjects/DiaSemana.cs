using HealthMed.Domain.Exceptions;

public sealed record DiaSemana
{
    private static readonly string[] Dias = 
        { "Domingo", "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira", "Sexta-feira", "Sábado" };

    public int Valor { get; }

    public DiaSemana(int valor)
    {
        if (valor is < 0 or > 6)
            throw new DomainException("Dia da semana inválido. Deve estar entre 0 (Domingo) e 6 (Sábado).");

        Valor = valor;
    }

    public override string ToString() => Dias[Valor];

    public static readonly DiaSemana Domingo = new(0);
    public static readonly DiaSemana Segunda = new(1);
    public static readonly DiaSemana Terca = new(2);
    public static readonly DiaSemana Quarta = new(3);
    public static readonly DiaSemana Quinta = new(4);
    public static readonly DiaSemana Sexta = new(5);
    public static readonly DiaSemana Sabado = new(6);

    public static DiaSemana DeNumero(int valor) => new(valor);
    
    public static implicit operator int(DiaSemana dia) => dia.Valor;
    public static implicit operator DiaSemana(int valor) => new(valor);
}