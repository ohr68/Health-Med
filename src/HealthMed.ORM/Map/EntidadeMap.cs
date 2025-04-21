using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.ORM.Map;

public class EntidadeMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entidade
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(u => u.Id)
            .HasColumnType("uniqueidentifier")
            .IsRequired();
        
        builder
            .Property(p => p.CriadoEm)
            .HasColumnType("datetime2(7)")
            .IsRequired();

        builder
            .Property(p => p.AtualizadoEm)
            .HasColumnType("datetime2(7)");

        builder
            .Property(p => p.Apagado)
            .IsRequired();

        builder.HasQueryFilter(p => !p.Apagado);
    }
}