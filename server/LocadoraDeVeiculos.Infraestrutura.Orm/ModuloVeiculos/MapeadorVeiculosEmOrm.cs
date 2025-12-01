using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloVeiculos;

public class MapeadorVeiculosEmOrm : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> builder)
    {
        builder.ToTable("TBVeiculos");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Placa)
            .HasColumnType("nvarchar(20)")
            .IsRequired();

        builder.Property(x => x.Modelo)
            .HasColumnType("nvarchar(50)")
            .IsRequired();

        builder.Property(x => x.Marca)
            .HasColumnType("nvarchar(50)")
            .IsRequired();

        builder.Property(x => x.Cor)
            .HasColumnType("nvarchar(50)")
            .IsRequired();

        builder.Property(x => x.TipoCombustivel)
            .IsRequired();

        builder.Property(x => x.CapacidadeTanque)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasOne(x => x.GrupoVeiculo)
            .WithMany(x => x.Veiculos)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
