using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Models.ViewModels;
using HealthMed.Domain.Entities;

namespace HealthMed.Application.Interfaces.Service;

public interface IMedicoAppService
{
    Task<MedicoViewModel?> ObterPorId(Guid medicoId, CancellationToken cancellationToken = default);
    Task<MedicoViewModel?> ObterPorCrm(string crm, CancellationToken cancellationToken = default);
    Task<IEnumerable<MedicoViewModel>?> ObterTodos(CancellationToken cancellationToken = default);
    Task Cadastrar(CadastroMedicoInputModel input, CancellationToken cancellationToken = default);
    Task Atualizar(Guid medicoId, AtualizacaoMedicoInputModel input, CancellationToken cancellationToken = default);
    Task Excluir(Guid medicoId, CancellationToken cancellationToken = default);
}