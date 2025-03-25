using Bogus;
using FluentAssertions;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.ValueObjects;

namespace HealthMed.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Fact(DisplayName = "Validar email válido.")]
    public void Email_Validar_EmailValido()
    {
        //Arrange
        var emailValido = new Faker().Person.Email;
        
        //Act
        var email = new Email(emailValido);
     
        email.Valor.Should().Be(emailValido);
    }
    
    [Fact(DisplayName = "Validar email inválido.")]
    public void Email_Validar_EmailInvalido()
    {
        //Arrange
        var emailInvalido = "email_inválido";
        
        //Act && Assert
        Assert.Throws<DomainException>(() => new Email(emailInvalido));
    }
}