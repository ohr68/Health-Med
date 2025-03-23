using FluentAssertions;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.ValueObjects;

namespace HealthMed.Domain.Tests.ValueObjects;

public class HoraTests
{
    [Fact(DisplayName = "Validar hora válida.")]
    public void Hora_Validar_HoraValido()
    {
        //Arrange
        var horaValida = 1;
        
        //Act
        var hora = new Hora(horaValida);
     
        //Assert
        hora.Valor.Should().Be(horaValida);
    }
    
    [Fact(DisplayName = "Validar hora inválido.")]
    public void Hora_Validar_HoraInvalido()
    {
        //Arrange
        var horaValida = 24;
        
        //Act && Assert
        Assert.Throws<DomainException>(() => new Hora(horaValida));
    }
}