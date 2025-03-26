using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.ORM.Map;

public class EspecialidadeMap : IEntityTypeConfiguration<Especialidade>
{
    public void Configure(EntityTypeBuilder<Especialidade> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder
            .Property(m => m.Nome)
            .HasMaxLength(200)
            .IsRequired();
        
        builder
            .Property(m => m.CriadoEm)
            .IsRequired();
        
        builder
            .Property(m => m.AtualizadoEm)
            .IsRequired(false);
        
        builder
            .Property(m => m.Apagado)
            .IsRequired();
        
        builder.HasQueryFilter(m => !m.Apagado);
        
        builder
            .HasIndex(p => p.Nome)
            .HasDatabaseName("IX_Especialidade_Nome");
        
        builder.ToTable("Especialidades");
    }
}