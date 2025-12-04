using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxaServico;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;

public class LocadoraDeVeiculosDbContext(
    DbContextOptions options, 
    ITenantProvider? tenantProvider = null)
    : IdentityDbContext<Usuario, Cargo, Guid>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (tenantProvider is not null)
        {
            modelBuilder.Entity<GrupoVeiculo>().HasQueryFilter(m => m.EmpresaId == tenantProvider.EmpresaId && !m.Excluido);
            modelBuilder.Entity<Veiculo>().HasQueryFilter(m => m.EmpresaId == tenantProvider.EmpresaId && !m.Excluido);
            modelBuilder.Entity<Condutor>().HasQueryFilter(m => m.EmpresaId == tenantProvider.EmpresaId && !m.Excluido);
            modelBuilder.Entity<Cliente>().HasQueryFilter(m => m.EmpresaId == tenantProvider.EmpresaId && !m.Excluido);
            modelBuilder.Entity<PlanoCobranca>().HasQueryFilter(m => m.EmpresaId == tenantProvider.EmpresaId && !m.Excluido);
            modelBuilder.Entity<TaxaServico>().HasQueryFilter(m => m.EmpresaId == tenantProvider.EmpresaId && !m.Excluido);
            modelBuilder.Entity<ConfiguracaoPreco>().HasQueryFilter(m => m.EmpresaId == tenantProvider.EmpresaId && !m.Excluido);
            modelBuilder.Entity<Aluguel>().HasQueryFilter(m => m.EmpresaId == tenantProvider.EmpresaId && !m.Excluido);
        }

        modelBuilder.ApplyConfiguration(new MapeadorGrupoVeiculosEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorVeiculosEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorCondutorEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorCliente());
        modelBuilder.ApplyConfiguration(new MapeadorPlanoCobrancaEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorTaxaServicoEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorConfiguracaoPrecoEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorAluguelEmOrm());

        base.OnModelCreating(modelBuilder);
    }
}

