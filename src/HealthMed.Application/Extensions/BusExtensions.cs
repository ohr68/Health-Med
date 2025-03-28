using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Messaging;

namespace HealthMed.Application.Extensions;

public static class BusExtensions
{
    public static async Task DispararEventos<T>(this IBusService busService, T entidade) where T : Entidade
    {
        if (entidade.PossuiEventos())
            await Task.WhenAll(entidade.Eventos.Select(evento => busService.Publish(evento)));
    }
}