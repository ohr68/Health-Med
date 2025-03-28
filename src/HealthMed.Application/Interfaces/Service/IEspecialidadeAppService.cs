using HealthMed.Application.Models.ViewModels;

namespace HealthMed.Application.Interfaces.Service;

public interface IEspecialidadeAppService
{
    Task<IEnumerable<EspecialidadeViewModel>?> ObterTodas(CancellationToken cancellationToken = default);
}