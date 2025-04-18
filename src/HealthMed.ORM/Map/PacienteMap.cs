using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.ORM.Map;

public class PacienteMap : IEntityTypeConfiguration<Paciente>
{
    public void Configure(EntityTypeBuilder<Paciente> builder)
    {
        builder.ToTable("Pacientes");
        
        builder
            .Property(p => p.Nome)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.OwnsOne(p => p.Email, email =>
        {
            email.Property(e => e.Valor)
                .HasColumnName("Email")
                .HasMaxLength(200)
                .IsRequired();
            
            email.HasIndex(e => e.Valor)
                .HasDatabaseName("IX_Paciente_Email")
                .IsUnique();
        });
        
        builder.OwnsOne(p => p.Cpf, cpf =>
        {
            cpf.Property(e => e.Valor)
                .HasColumnName("Cpf")
                .HasMaxLength(11)
                .IsRequired();
            
            cpf.HasIndex(e => e.Valor)
                .HasDatabaseName("IX_Paciente_Cpf")
                .IsUnique();
        });
    }
}