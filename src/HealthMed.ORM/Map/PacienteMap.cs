using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.ORM.Map;

public class PacienteMap : IEntityTypeConfiguration<Paciente>
{
    public void Configure(EntityTypeBuilder<Paciente> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Nome)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.OwnsOne(p => p.Email, email =>
        {
            email.Property(e => e.Valor)
                .HasColumnName("Email") // Mapeamento do campo de valor do Email
                .HasMaxLength(200)
                .IsRequired();
            
            email.HasIndex(e => e.Valor)
                .HasDatabaseName("IX_Paciente_Email")
                .IsUnique();
        });
        
        builder
            .Property(p => p.CriadoEm)
            .IsRequired();
        
        builder
            .Property(p => p.AtualizadoEm)
            .IsRequired(false);
        
        builder
            .Property(p => p.Apagado)
            .IsRequired();
        
        builder.HasQueryFilter(p => !p.Apagado);
        
        builder.ToTable("Pacientes");
    }
}