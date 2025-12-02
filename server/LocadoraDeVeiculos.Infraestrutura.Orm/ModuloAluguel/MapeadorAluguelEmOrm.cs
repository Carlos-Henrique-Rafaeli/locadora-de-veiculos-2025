using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;

public class MapeadorAluguelEmOrm : IEntityTypeConfiguration<Aluguel>
{
    public void Configure(EntityTypeBuilder<Aluguel> builder)
    {
        builder.ToTable("TBAluguel");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.EstaAberto)
            .IsRequired();

        builder.Property(x => x.ValorFinal)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .IsRequired();

        builder.HasOne(x => x.Condutor)
            .WithMany()
            .IsRequired()
            .HasForeignKey("CondutorId")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.GrupoVeiculo)
            .WithMany()
            .IsRequired()
            .HasForeignKey("GrupoVeiculoId")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Veiculo)
            .WithMany()
            .IsRequired()
            .HasForeignKey("VeiculoId")
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.DataEntrada)
            .IsRequired();

        builder.Property(x => x.DataRetorno)
            .IsRequired();

        builder.HasOne(x => x.PlanoCobranca)
            .WithMany()
            .IsRequired()
            .HasForeignKey("PlanoCobrancaId")
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.TaxasServicos)
            .WithMany();

        builder
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
