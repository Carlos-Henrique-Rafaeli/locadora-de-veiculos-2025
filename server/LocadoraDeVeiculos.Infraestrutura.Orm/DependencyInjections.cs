using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCondutor;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloConfiguracao;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxaServico;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloVeiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LocadoraDeVeiculos.Infraestrutura.Orm;

public static class DependencyInjections
{
    public static IServiceCollection AddCamadaInfraestruturaOrm(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);
        services.ConfigureRepositories();

        return services;
    }

    public static void ConfigureDbContext(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var connectionString = config["SQL_CONNECTION_STRING"];

        if (connectionString == null)
            throw new ArgumentNullException("'SQL_CONNECTION_STRING' não foi fornecida para o ambiente.");

        services.AddDbContext<LocadoraDeVeiculosDbContext, LocadoraDeVeiculosDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(connectionString, dbOptions =>
            {
                dbOptions.EnableRetryOnFailure(3);
            });
        });
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepositorioGrupoVeiculos, RepositorioGrupoVeiculosEmOrm>();
        services.AddScoped<IRepositorioVeiculo, RepositorioVeiculoEmOrm>();
        services.AddScoped<IRepositorioCondutor, RepositorioCondutorEmOrm>();
        services.AddScoped<IRepositorioCliente, RepositorioClienteEmOrm>();
        services.AddScoped<IRepositorioPlanoCobranca, RepositorioPlanoCobrancaEmOrm>();
        services.AddScoped<IRepositorioTaxaServico, RepositorioTaxaServicoEmOrm>();
        services.AddScoped<IRepositorioConfiguracaoPreco, RepositorioConfiguracaoPrecoEmOrm>();
        services.AddScoped<IRepositorioAluguel, RepositorioAluguelEmOrm>();
    }
}
