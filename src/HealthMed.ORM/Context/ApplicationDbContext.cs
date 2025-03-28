using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HealthMed.ORM.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Administrator> Administradores { get; protected set; }
    public DbSet<Paciente> Pacientes { get; protected set; }
    public DbSet<Medico> Medicos { get; protected set; }
    public DbSet<Consulta> Consultas { get; protected set; }
    public DbSet<Especialidade> Especialidades { get; protected set; }
    public DbSet<DisponibilidadeMedico> DisponibilidadeMedicos { get; protected set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken) =>
        await SaveChangesAsync(cancellationToken) > 0;
}