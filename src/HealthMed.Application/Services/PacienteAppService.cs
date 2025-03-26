using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Models.InputModels;
using HealthMed.Domain.Interfaces.Messaging;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.Domain.Interfaces.UnitOfWork;

namespace HealthMed.Application.Services;

public class PacienteAppService(IUnitOfWork uow, IPacienteRepository pacienteRepository, IBusService busService)
    : IPacienteAppService
{
    public Task Cadastrar(CadastroPacienteInputModel input)
    {
        throw new NotImplementedException();
    }

    public Task Atualizar(AtualizacaoPacienteInputModel input)
    {
        throw new NotImplementedException();
    }

    public Task Excluir(Guid pacienteId)
    {
        throw new NotImplementedException();
    }
}