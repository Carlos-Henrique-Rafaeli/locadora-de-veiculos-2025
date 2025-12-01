using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;

public class MapeadorPlanoCobrancaEmOrm : IEntityTypeConfiguration<PlanoCobranca>
{
    public void Configure(EntityTypeBuilder<PlanoCobranca> builder)
    {
        builder.ToTable("TBPlanoCobranca");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.TipoPlano)
            .IsRequired();

        builder.HasOne(x => x.GrupoVeiculo)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.ValorDiario)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.ValorKm)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.KmIncluso)
            .HasColumnType("int");

        builder.Property(x => x.ValorKmExcedente)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.ValorFixo)
            .HasColumnType("decimal(18,2)");

        builder
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
