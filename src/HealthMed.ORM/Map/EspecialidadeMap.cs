using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.ORM.Map;

public class EspecialidadeMap : IEntityTypeConfiguration<Especialidade>
{
    public void Configure(EntityTypeBuilder<Especialidade> builder)
    {
        builder.ToTable("Especialidades");
        
        builder
            .Property(m => m.Nome)
            .HasMaxLength(200)
            .IsRequired();
        
        builder
            .HasIndex(p => p.Nome)
            .HasDatabaseName("IX_Especialidade_Nome");
    }
}