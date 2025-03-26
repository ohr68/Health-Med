namespace HealthMed.Application.Models.InputModels;

public record CadastroPacienteInputModel(string Nome, string Email);

public record AtualizacaoPacienteInputModel(string Nome);