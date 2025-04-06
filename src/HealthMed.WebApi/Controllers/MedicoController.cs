using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.WebApi.Controllers
{
    /// <summary>
    /// Médicos
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="medicoAppService"></param>
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController(ILogger<MedicoController> logger, IMedicoAppService medicoAppService) : ControllerBase
    {
        /// <summary>
        /// Obter médico por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MedicoViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> ObterPorId([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            logger.LogTrace("Obtendo médico por Id = {0}", id);

            var medico = await medicoAppService.ObterPorId(id, cancellationToken);

            logger.LogTrace(medico is null ? "Médico Id {0} não encontrado" : "Médico Id {0} encontrado", id);

            return Ok(medico);
        }

        /// <summary>
        /// Obter médico por crm
        /// </summary>
        /// <param name="crm"></param>
        /// <returns></returns>
        [HttpGet("ObterPorCrm/{crm}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MedicoViewModel))]
        public async Task<IActionResult> ObterPorCrm([FromRoute] string crm, CancellationToken cancellationToken)
        {
            logger.LogTrace("Obtendo médico por crm = {0}", crm);

            var medico = await medicoAppService.ObterPorCrm(crm, cancellationToken);

            logger.LogTrace(medico is null ? "Médico crm {0} não encontrado" : "Médico crm {0} encontrado", crm);

            return Ok(medico);
        }

        /// <summary>
        /// Obter médicos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MedicoViewModel>))]
        public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken)
        {
            logger.LogTrace("Obtendo médicos");

            var medicos = await medicoAppService.ObterTodos(cancellationToken);

            logger.LogTrace(!medicos?.Any() ?? true ? "Nenhum médico encontrado" : "{0} médicos encontrados",
                medicos?.Count());

            return Ok(medicos);
        }

        /// <summary>
        /// Cadastrar médico
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Cadastrar([FromBody] CadastroMedicoInputModel input,
            CancellationToken cancellationToken)
        {
            logger.LogTrace("Cadastrando médico");

            await medicoAppService.Cadastrar(input, cancellationToken);

            logger.LogTrace("Médico cadastrado com sucesso");

            return Created();
        }

        /// <summary>
        /// Atualizar médico
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Atualizar([FromRoute] Guid id, [FromBody] AtualizacaoMedicoInputModel input,
            CancellationToken cancellationToken)
        {
            logger.LogTrace("Atualizando médico");

            await medicoAppService.Atualizar(id, input, cancellationToken);

            logger.LogTrace("Médico atualizado com sucesso");

            return NoContent();
        }

        /// <summary>
        /// Atualizar disponibilidade médico
        /// </summary>
        /// <returns></returns>
        [HttpPatch("AtualizarDisponibilidade/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> AtualizarDisponibilidade([FromRoute] Guid id,
            [FromBody] IEnumerable<DisponibilidadeMedicoInputModel> input,
            CancellationToken cancellationToken)
        {
            logger.LogTrace("Atualizando disponibilidade médico");

            await medicoAppService.AtualizarDisponibilidade(id, input, cancellationToken);

            logger.LogTrace("Disponibilidade médico atualizado com sucesso");

            return NoContent();
        }

        /// <summary>
        /// Excluir médico
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Excluir([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            logger.LogTrace("Excluindo médico");

            await medicoAppService.Excluir(id, cancellationToken);

            logger.LogTrace("Médico excluído com sucesso");

            return NoContent();
        }
    }
}