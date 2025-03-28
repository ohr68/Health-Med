namespace HealthMed.Application.Models.InputModels;

public record AgendarConsultaInputModel(Guid PacienteId, Guid MedicoId, DateTime Horario);

public record CancelarConsultaInputModel(string JustificativaCancelamento);