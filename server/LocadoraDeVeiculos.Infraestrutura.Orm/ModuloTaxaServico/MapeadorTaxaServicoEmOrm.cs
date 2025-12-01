using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxaServico;

public class MapeadorTaxaServicoEmOrm : IEntityTypeConfiguration<TaxaServico>
{
    public void Configure(EntityTypeBuilder<TaxaServico> builder)
    {
        builder.ToTable("TBTaxaServico");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Valor)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.TipoCobranca)
            .IsRequired();

        builder
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
