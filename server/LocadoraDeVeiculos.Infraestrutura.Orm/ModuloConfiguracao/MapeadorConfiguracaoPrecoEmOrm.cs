using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloConfiguracao;

public class MapeadorConfiguracaoPrecoEmOrm : IEntityTypeConfiguration<ConfiguracaoPreco>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoPreco> builder)
    {
        builder.ToTable("TBConfiguracaoPreco");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Gasolina)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(6.99)
            .IsRequired();

        builder.Property(x => x.Diesel)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(5.99)
            .IsRequired();

        builder.Property(x => x.Etanol)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(4.99)
            .IsRequired();

        builder
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
