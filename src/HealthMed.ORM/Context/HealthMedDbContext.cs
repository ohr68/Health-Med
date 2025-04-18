using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.UnitOfWork;
using HealthMed.ORM.Map;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HealthMed.ORM.Context;

public class HealthMedDbContext(DbContextOptions<HealthMedDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Administrator> Administradores { get; protected set; }
    public DbSet<Paciente> Pacientes { get; protected set; }
    public DbSet<Medico> Medicos { get; protected set; }
    public DbSet<Usuario> Usuarios { get; protected set; }
    public DbSet<Consulta> Consultas { get; protected set; }
    public DbSet<Especialidade> Especialidades { get; protected set; }
    public DbSet<DisponibilidadeMedico> DisponibilidadeMedicos { get; protected set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HealthMedDbContext).Assembly);
        
        Console.WriteLine("Applying Entity Configurations...");

        var baseConfigurations = new Dictionary<Type, Type>
        {
            { typeof(Entidade), typeof(EntidadeMap<>) }
        };

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var type = entityType.ClrType;

            foreach (var (baseType, configTypeDefinition) in baseConfigurations)
            {
                if (!baseType.IsAssignableFrom(type) || type == baseType) continue;
                
                var configType = configTypeDefinition.MakeGenericType(type);
                var configInstance = Activator.CreateInstance(configType) 
                                     ?? throw new InvalidOperationException($"Could not create instance of {configType}");

                modelBuilder.ApplyConfiguration((dynamic)configInstance);
                Console.WriteLine($"✅ Applied {configTypeDefinition.Name} to {type.Name}");
            }
        }

        Console.WriteLine("✅ All entity configurations applied successfully!");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken) =>
        await SaveChangesAsync(cancellationToken) > 0;
}