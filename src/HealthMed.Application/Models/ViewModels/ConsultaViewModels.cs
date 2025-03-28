namespace HealthMed.Application.Models.ViewModels;

public record ConsultaViewModel(
    DateTime Horario,
    string Status,
    string? JustificativaCancelamento,
    decimal Valor,
    PacienteViewModel Paciente,
    MedicoViewModel Medico);