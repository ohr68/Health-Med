using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.ORM.Map;

public class UsuarioMap : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        
        builder.Property(u => u.Senha)
            .HasColumnType("varchar(max)")
            .IsRequired();
        
        builder.Property(u => u.LoginLiberado)
            .HasColumnType("bit")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(u => u.Status)
            .HasColumnType("int")
            .IsRequired();
        
        builder.Property(u => u.TipoUsuario)
            .HasColumnType("int")
            .IsRequired();

        builder.HasMany(u => u.Pacientes)
            .WithOne(p => p.Usuario)
            .HasForeignKey(p => p.UsuarioId);
        
        builder.HasMany(u => u.Medicos)
            .WithOne(m => m.Usuario)
            .HasForeignKey(m => m.UsuarioId);
    }
}