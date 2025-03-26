using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.ORM.Map;

public class DisponibilidadeMedicoMap : IEntityTypeConfiguration<DisponibilidadeMedico>
{
    public void Configure(EntityTypeBuilder<DisponibilidadeMedico> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .Property(p => p.MedicoId)
            .IsRequired();

        builder.OwnsOne(p => p.DiaSemana, diaSemana => 
            diaSemana.Property(p => p.Valor)
                .HasColumnName("DiaSemana")
                .IsRequired());

        builder.OwnsOne(p => p.HoraInicio, hora => 
            hora.Property(p => p.Valor)
                .HasColumnName("HoraInicio")
                .IsRequired());

        builder.OwnsOne(p => p.HoraFim, hora => 
            hora.Property(p => p.Valor)
                .HasColumnName("HoraFim")
                .IsRequired());

        builder.HasOne(p => p.Medico)
            .WithMany(p => p.Disponibilidade)
            .HasForeignKey(p => p.MedicoId);

        builder.HasQueryFilter(p => !p.Apagado);

        builder.ToTable("DisponibilidadeMedico");
    }
}