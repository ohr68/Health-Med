﻿namespace HealthMed.Application.Models.ViewModels;

public record MedicoViewModel(
    string Nome,
    string Email,
    string Crm,
    decimal ValorConsulta,
    EspecialidadeViewModel Especialidade,
    List<DisponibilidadeMedicoViewModel> Disponibilidade);

public record DisponibilidadeMedicoViewModel(int DiaSemana, int HoraInicio, int HoraFim);