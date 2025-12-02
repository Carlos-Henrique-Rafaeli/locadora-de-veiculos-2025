using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCondutor;

public class MapeadorCondutorEmOrm : IEntityTypeConfiguration<Condutor>
{
    public void Configure(EntityTypeBuilder<Condutor> builder)
    {
        builder.ToTable("TBCondutor");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasOne(x => x.Cliente)
            .WithMany();

        builder.Property(x => x.ClienteCondutor)
            .IsRequired();

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Cpf)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(x => x.Cnh)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(x => x.ValidadeCnh)
            .IsRequired();

        builder.Property(x => x.Telefone)
            .IsRequired()
            .HasMaxLength(15);

        builder
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
