using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Tests.Fixture;

public class ConsultaFixture
{
    public Consulta CriarConsulta(Guid pacienteId, Guid medicoId, DateTime horario)
    {
        return new Consulta(pacienteId, medicoId, horario);
    }
    
    public DateTime ProximaSegundaAsOito()
    {
        DateTime hoje = DateTime.Now;
        
        // Calcula quantos dias faltam para a próxima segunda-feira
        int diasParaSegunda = ((int)DayOfWeek.Monday - (int)hoje.DayOfWeek + 7) % 7;
        
        // Se hoje já for segunda, avançamos para a próxima
        if (diasParaSegunda == 0)
            diasParaSegunda = 7;

        // Criamos a data ajustando para a próxima segunda às 08:00:00
        return hoje.Date.AddDays(diasParaSegunda).AddHours(8);
    }
}