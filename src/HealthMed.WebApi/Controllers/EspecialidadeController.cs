using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.WebApi.Controllers
{
    /// <summary>
    /// Especialidades dos médicos
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="especialidadeAppService"></param>
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadeController(
        ILogger<EspecialidadeController> logger,
        IEspecialidadeAppService especialidadeAppService) : ControllerBase
    {
        /// <summary>
        /// Obter especialidades
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EspecialidadeViewModel>))]
        [AllowAnonymous]
        public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken)
        {
            logger.LogTrace("Obtendo especialidades");

            var especialidades = await especialidadeAppService.ObterTodas(cancellationToken);

            logger.LogTrace(
                !especialidades?.Any() ?? true ? "Nenhuma especialidade encontrada" : "{0} especialidades encontradas",
                especialidades?.Count());

            return Ok(especialidades);
        }
    }
}