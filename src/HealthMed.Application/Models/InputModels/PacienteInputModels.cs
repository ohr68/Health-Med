namespace HealthMed.Application.Models.InputModels;

public record CadastroPacienteInputModel(string Nome, string Email, string Cpf, string Senha);

public record AtualizacaoPacienteInputModel(string Nome);