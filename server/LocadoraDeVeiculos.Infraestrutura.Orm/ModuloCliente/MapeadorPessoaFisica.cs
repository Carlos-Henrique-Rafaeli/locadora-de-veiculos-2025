using LocadoraDeVeiculos.Dominio.ModuloCliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;

public class MapeadorPessoaFisica : IEntityTypeConfiguration<PessoaFisica>
{
    public void Configure(EntityTypeBuilder<PessoaFisica> builder)
    {
        builder.ToTable("TBPessoaFisica");

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

        builder.Property(x => x.Cpf)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(x => x.Rg)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(x => x.Cnh)
            .IsRequired()
            .HasMaxLength(11);

        builder.HasOne(pf => pf.PessoaJuridica)
            .WithOne()
            .HasForeignKey<PessoaFisica>(pf => pf.PessoaJuridicaId)
            .IsRequired(false);

        builder
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
