using FluentValidation;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LocadoraDeVeiculos.Aplicacao;

public static class DependencyInjection
{
    public static IServiceCollection AddCamadaAplicacao(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblyAplicacao = typeof(DependencyInjection).Assembly;
        var assemblyDominio = typeof(ValidadorVeiculo).Assembly;

        services.AddValidatorsFromAssembly(assemblyDominio);
        services.AddValidatorsFromAssembly(assemblyAplicacao);

        var licenseKey = configuration["LUCKYPENNY_LICENSE_KEY"];

        if (string.IsNullOrWhiteSpace(licenseKey))
            throw new Exception("A variável LUCKYPENNY_LICENSE_KEY não foi fornecida.");

        services.AddMediatR(config =>
        {
            config.LicenseKey = licenseKey;
            config.RegisterServicesFromAssembly(assemblyDominio);
            config.RegisterServicesFromAssembly(assemblyAplicacao);
        });

        return services;
    }
}
