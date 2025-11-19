using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloGrupoVeiculos;

public class MapeadorGrupoVeiculosEmOrm : IEntityTypeConfiguration<GrupoVeiculo>
{
    public void Configure(EntityTypeBuilder<GrupoVeiculo> builder)
    {
        builder.ToTable("TBGrupoVeiculos");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Nome)
            .HasColumnType("nvarchar(100)")
            .IsRequired();

        builder
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
