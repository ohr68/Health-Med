using FluentAssertions;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Exceptions;

namespace HealthMed.Domain.Tests.Entities;

public class EspecialidadeTests
{
    [Fact(DisplayName = "Criar especialidade válida.")]
    public void Especialidade_Criar_QuandoValida()
    {
        //Arrage
        var nomeEspecialidade = "Pediatría";
        
        //Act
        var especialidade = new Especialidade(nomeEspecialidade);
        
        //Assert
        especialidade.Nome.Should().Be(nomeEspecialidade);
    }
    
    [Fact(DisplayName = "Não deve criar especialidade quando inválida.")]
    public void Especialidade_NaoDeveCriar_QuandoInvalida()
    {
        //Arrage
        var nomeEspecialidade = "";
        
        //Act && Assert
        Assert.Throws<DomainException>(() => new Especialidade(nomeEspecialidade));
    }
}