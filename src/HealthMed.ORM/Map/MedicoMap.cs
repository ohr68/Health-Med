using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.ORM.Map;

public class MedicoMap : IEntityTypeConfiguration<Medico>
{
    public void Configure(EntityTypeBuilder<Medico> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder
            .Property(m => m.Nome)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.OwnsOne(p => p.Email, email =>
        {
            email.Property(e => e.Valor)
                .HasColumnName("Email")
                .HasMaxLength(200)
                .IsRequired();
            
            email.HasIndex(e => e.Valor)
                .HasDatabaseName("IX_Medico_Email")
                .IsUnique();
        });
        
        builder
            .Property(m => m.Crm)
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .Property(m => m.ValorConsulta)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder
            .Property(m => m.EspecialidadeId)
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

        builder.HasOne(p => p.Especialidade)
            .WithMany(p => p.Medicos)
            .HasForeignKey(p => p.EspecialidadeId);
        
        builder
            .HasIndex(m => m.Crm)
            .HasDatabaseName("IX_Medico_Crm")
            .IsUnique();
        
        builder
            .HasIndex(m => m.Nome)
            .HasDatabaseName("IX_Medico_Nome");
        
        builder
            .HasIndex(m => m.EspecialidadeId)
            .HasDatabaseName("IX_Medico_EspecialidadeId");

        builder.Navigation(x => x.Especialidade)
            .AutoInclude();
        
        builder.Navigation(x => x.Disponibilidade)
            .AutoInclude();
        
        builder.HasQueryFilter(m => !m.Apagado);
        
        builder.ToTable("Medicos");
    }
}