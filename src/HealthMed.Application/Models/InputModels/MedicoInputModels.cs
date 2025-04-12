namespace HealthMed.Application.Models.InputModels;

public record CadastroMedicoInputModel(
    string Nome,
    string Email,
    string Crm,
    decimal ValorConsulta,
    Guid EspecialidadeId,
    string Senha,
    IEnumerable<DisponibilidadeMedicoInputModel>? Disponibilidade);

public record DisponibilidadeMedicoInputModel(int DiaSemana, int HoraInicio, int HoraFim);

public record AtualizacaoMedicoInputModel(string Nome, decimal ValorConsulta);