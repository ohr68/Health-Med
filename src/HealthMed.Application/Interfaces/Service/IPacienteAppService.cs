using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Models.ViewModels;

namespace HealthMed.Application.Interfaces.Service;

public interface IPacienteAppService
{
    Task<PacienteViewModel?> ObterPorId(Guid pacienteId, CancellationToken cancellationToken = default);
    Task Cadastrar(CadastroPacienteInputModel input, CancellationToken cancellationToken = default);
    Task Atualizar(Guid pacienteId, AtualizacaoPacienteInputModel input,  CancellationToken cancellationToken = default);
    Task Excluir(Guid pacienteId,  CancellationToken cancellationToken = default);
}