using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Models.ViewModels;

namespace HealthMed.Application.Interfaces.Service;

public interface IConsultaAppService
{
    Task<ConsultaViewModel> ObterPorId(Guid consultaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ConsultaViewModel>?> ObterConsultasPaciente(Guid pacienteId,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<ConsultaViewModel>?> ObterConsultasMedico(Guid medicoId,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<ConsultaViewModel>?> ObterConsultasPendentesMedico(Guid medicoId,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<MedicoViewModel>?> ObterMedicos(Guid? especialidadeId = null,
        CancellationToken cancellationToken = default);
    Task Agendar(AgendarConsultaInputModel input, CancellationToken cancellationToken = default);
    Task Cancelar(Guid consultaId, CancelarConsultaInputModel input, CancellationToken cancellationToken = default);
    Task Aceitar(Guid consultaId, CancellationToken cancellationToken = default);
    Task Recusar(Guid consultaId, CancellationToken cancellationToken = default);
}