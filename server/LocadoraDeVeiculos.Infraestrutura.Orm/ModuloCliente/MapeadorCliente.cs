using LocadoraDeVeiculos.Dominio.ModuloCliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;

public class MapeadorCliente : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("TBCliente");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Telefone)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(x => x.Cpf)
            .IsRequired(false)
            .HasMaxLength(14);

        builder.Property(x => x.Cnpj)
            .IsRequired(false)
            .HasMaxLength(19);

        builder.Property(x => x.Estado)
            .IsRequired();

        builder.Property(x => x.Cidade)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Bairro)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Rua)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Numero)
            .IsRequired();

        builder
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
