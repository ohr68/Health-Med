using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Models.ViewModels;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Services;
using Mapster;

namespace HealthMed.Application.Services;

internal class EspecialidadeAppService(IEspecialidadeService especialidadeService) : IEspecialidadeAppService
{
    public async Task<IEnumerable<EspecialidadeViewModel>?> ObterTodas(CancellationToken cancellationToken = default)
    {
        var especialidades = await especialidadeService.ObterTodas(cancellationToken);

        return especialidades?.Adapt<IEnumerable<EspecialidadeViewModel>>();
    }
}