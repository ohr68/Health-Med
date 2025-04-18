using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.WebApi.Controllers;

/// <summary>
/// Pacientes
/// </summary>
/// <param name="logger"></param>
/// <param name="pacienteAppService"></param>
[Route("api/[controller]")]
[ApiController]
public class PacienteController(ILogger<PacienteController> logger, IPacienteAppService pacienteAppService) : Controller
{
    /// <summary>
    /// Obter paciente por id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PacienteViewModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ObterPorId([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        logger.LogTrace("Obtendo paciente por Id = {0}", id);

        var paciente = await pacienteAppService.ObterPorId(id, cancellationToken);

        logger.LogTrace(paciente is null ? "Paciente Id {0} não encontrado" : "Paciente Id {0} encontrado", id);

        return Ok(paciente);
    }

    /// <summary>
    /// Cadastrar paciente
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
    [AllowAnonymous]
    public async Task<IActionResult> Cadastrar([FromBody] CadastroPacienteInputModel input,
        CancellationToken cancellationToken)
    {
        logger.LogTrace("Cadastrando paciente");
        
        await pacienteAppService.Cadastrar(input, cancellationToken);
        
        logger.LogTrace("Paciente cadastrado com sucesso");
        
        return Created();
    }

    /// <summary>
    /// Atualizar paciente
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Atualizar([FromRoute] Guid id, [FromBody] AtualizacaoPacienteInputModel input,
        CancellationToken cancellationToken)
    {
        logger.LogTrace("Atualizando paciente");
        
        await pacienteAppService.Atualizar(id, input, cancellationToken);
        
        logger.LogTrace("Paciente atualizado com sucesso");
        
        return NoContent();
    }

    /// <summary>
    /// Excluir paciente
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Excluir([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        logger.LogTrace("Excluindo paciente");
        
        await pacienteAppService.Excluir(id, cancellationToken);
     
        logger.LogTrace("Paciente excluído com sucesso");
        
        return NoContent();
    }
}