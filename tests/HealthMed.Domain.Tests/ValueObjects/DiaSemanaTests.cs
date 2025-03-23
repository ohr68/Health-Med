using FluentAssertions;
using HealthMed.Domain.Exceptions;

namespace HealthMed.Domain.Tests.ValueObjects;

public class DiaSemanaTests
{
    [Fact(DisplayName = "Validar dia da semana válido.")]
    public void DiaSemana_Validar_DiaSemanaValido()
    {
        //Arrange
        var dia = 1;
        
        //Act
        var diaSemana = new DiaSemana(dia);
     
        //Assert
        diaSemana.Valor.Should().Be(dia);
        diaSemana.ToString().Should().Be("Segunda-feira");
    }
    
    [Fact(DisplayName = "Validar dia da semana inválido.")]
    public void DiaSemana_Validar_DiaSemanaInvalido()
    {
        //Arrange
        var diaInvalido = 8;
        
        //Act && Assert
        Assert.Throws<DomainException>(() => new DiaSemana(diaInvalido));
    }
}