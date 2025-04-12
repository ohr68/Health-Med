using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.ORM.Map;

public class ConsultaMap : IEntityTypeConfiguration<Consulta>
{
    public void Configure(EntityTypeBuilder<Consulta> builder)
    {
        builder.ToTable("Consultas");
        
        builder
            .Property(p => p.PacienteId)
            .IsRequired();
        
        builder
            .Property(p => p.MedicoId)
            .IsRequired();
        
        builder
            .Property(p => p.Horario)
            .IsRequired();
        
        builder
            .Property(p => p.Status)
            .IsRequired();
        
        builder
            .Property(p => p.JustificativaCancelamento)
            .HasMaxLength(500)
            .IsRequired(false);
        
        builder
            .Property(p => p.Valor)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasOne(p => p.Paciente)
            .WithMany(p => p.Consultas)
            .HasForeignKey(p => p.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(p => p.Medico)
            .WithMany(p => p.Consultas)
            .HasForeignKey(p => p.MedicoId);

        builder.Navigation(x => x.Paciente)
            .AutoInclude();
        
        builder.Navigation(x => x.Medico)
            .AutoInclude();
    }
}