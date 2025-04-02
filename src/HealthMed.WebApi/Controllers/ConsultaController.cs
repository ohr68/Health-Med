using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.WebApi.Controllers
{
    /// <summary>
    /// Consultas médicas
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="consultaAppService"></param>
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultaController(ILogger<ConsultaController> logger, IConsultaAppService consultaAppService)
        : ControllerBase
    {
        /// <summary>
        /// Obter consulta por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConsultaViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> ObterPorId([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            logger.LogTrace("Obtendo consulta por Id = {0}", id);

            var consulta = await consultaAppService.ObterPorId(id, cancellationToken);

            return Ok(consulta);
        }

        /// <summary>
        /// Obter consultas do paciente
        /// </summary>
        /// <param name="pacienteId"></param>
        /// <returns></returns>
        [HttpGet("ObterConsultasPaciente/{pacienteId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ConsultaViewModel>))]
        public async Task<IActionResult> ObterConsultasPaciente([FromRoute] Guid pacienteId,
            CancellationToken cancellationToken)
        {
            logger.LogTrace("Obtendo consultas do paciente Id = {0}", pacienteId);

            var consultas = await consultaAppService.ObterConsultasPaciente(pacienteId, cancellationToken);

            logger.LogTrace(!consultas?.Any() ?? true ? "Nenhum consulta encontrada" : "{0} consultas encontradas",
                consultas?.Count());

            return Ok(consultas);
        }

        /// <summary>
        /// Obter consultas do médico
        /// </summary>
        /// <param name="medicoId"></param>
        /// <returns></returns>
        [HttpGet("ObterConsultasMedico/{medicoId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ConsultaViewModel>))]
        public async Task<IActionResult> ObterConsultasMedico([FromRoute] Guid medicoId,
            CancellationToken cancellationToken)
        {
            logger.LogTrace("Obtendo consultas do medico Id = {0}", medicoId);

            var consultas = await consultaAppService.ObterConsultasMedico(medicoId, cancellationToken);

            logger.LogTrace(!consultas?.Any() ?? true ? "Nenhum consulta encontrada" : "{0} consultas encontradas",
                consultas?.Count());

            return Ok(consultas);
        }

        /// <summary>
        /// Obter consultas pendentes do médico
        /// </summary>
        /// <param name="medicoId"></param>
        /// <returns></returns>
        [HttpGet("ObterConsultasPendentesMedico/{medicoId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ConsultaViewModel>))]
        public async Task<IActionResult> ObterConsultasPendentesMedico([FromRoute] Guid medicoId,
            CancellationToken cancellationToken)
        {
            logger.LogTrace("Obtendo consultas pendentes do medico Id = {0}", medicoId);

            var consultas = await consultaAppService.ObterConsultasPendentesMedico(medicoId, cancellationToken);

            logger.LogTrace(!consultas?.Any() ?? true ? "Nenhum consulta encontrada" : "{0} consultas encontradas",
                consultas?.Count());

            return Ok(consultas);
        }

        /// <summary>
        /// Obter médicos disponíveis para consulta
        /// </summary>
        /// <param name="especialidadeId"></param>
        /// <returns></returns>
        [HttpGet("ObterMedicos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MedicoViewModel>))]
        public async Task<IActionResult> ObterMedicos([FromQuery] Guid? especialidadeId,
            CancellationToken cancellationToken)
        {
            if (especialidadeId.HasValue)
                logger.LogTrace("Obtendo médicos com especialidade = {0}", especialidadeId);
            else
                logger.LogTrace("Obtendo médicos");

            var medicos = await consultaAppService.ObterMedicos(especialidadeId, cancellationToken);

            logger.LogTrace(!medicos?.Any() ?? true ? "Nenhum médico encontrado" : "{0} médicos encontrados",
                medicos?.Count());

            return Ok(medicos);
        }

        /// <summary>
        /// Agendar consulta médica
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Agendar([FromBody] AgendarConsultaInputModel input,
            CancellationToken cancellationToken)
        {
            logger.LogTrace("Paciente Id = {0} agendando consulta com Medico Id = {1}", input.PacienteId,
                input.MedicoId);

            await consultaAppService.Agendar(input, cancellationToken);

            logger.LogTrace("Consulta agendada com sucesso");

            return Created();
        }

        /// <summary>
        /// Cancelar consulta médica
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPatch("Cancelar/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Agendar([FromRoute] Guid id, [FromBody] CancelarConsultaInputModel input,
            CancellationToken cancellationToken)
        {
            logger.LogTrace("Cancelando consulta Id = {0}", id);

            await consultaAppService.Cancelar(id, input, cancellationToken);

            logger.LogTrace("Consulta cancelada com sucesso");

            return NoContent();
        }

        /// <summary>
        /// Médico aceitar consulta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("Aceitar/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Aceitar([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            logger.LogTrace("Aceitando consulta Id = {0}", id);

            await consultaAppService.Aceitar(id, cancellationToken);

            logger.LogTrace("Consulta aceita com sucesso");

            return NoContent();
        }

        /// <summary>
        /// Médico recusar consulta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("Recusar/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Recusar([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            logger.LogTrace("Recusando consulta Id = {0}", id);

            await consultaAppService.Aceitar(id, cancellationToken);

            logger.LogTrace("Consulta recusada com sucesso");

            return NoContent();
        }
    }
}