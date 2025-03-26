using HealthMed.Application.Models.InputModels;

namespace HealthMed.Application.Interfaces.Service;

public interface IPacienteAppService
{
    Task Cadastrar(CadastroPacienteInputModel input);
    Task Atualizar(AtualizacaoPacienteInputModel input);
    Task Excluir(Guid pacienteId);
}