using LocadoraDeVeiculos.Dominio.ModuloCliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;

public class MapeadorPessoaJuridica : IEntityTypeConfiguration<PessoaJuridica>
{
    public void Configure(EntityTypeBuilder<PessoaJuridica> builder)
    {
        builder.ToTable("TBPessoaJuridica");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Telefone)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(x => x.Endereco)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Cnpj)
            .IsRequired()
            .HasMaxLength(18);

        builder.HasOne(pj => pj.Condutor)
            .WithMany()
            .IsRequired();

        builder
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
