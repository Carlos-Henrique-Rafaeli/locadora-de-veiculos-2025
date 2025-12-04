using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloConfiguracao.Commands.Selecionar;

internal class SelecionarConfiguracaoPrecoRequestHandler(
    IRepositorioConfiguracaoPreco repositorioConfiguracaoPreco,
    ITenantProvider tenantProvider,
    LocadoraDeVeiculosDbContext contexto
) : IRequestHandler<SelecionarConfiguracaoPrecoRequest, Result<SelecionarConfiguracaoPrecoResponse>>
{
    public async Task<Result<SelecionarConfiguracaoPrecoResponse>> Handle(
        SelecionarConfiguracaoPrecoRequest request, CancellationToken cancellationToken)
    {
        var registros = await repositorioConfiguracaoPreco.SelecionarTodosAsync();

        var registro = registros.FirstOrDefault();

        if (registro == null)
        {
            var config = new ConfiguracaoPreco
            {
                Gasolina = 6.99m,
                Diesel = 5.99m,
                Etanol = 4.99m,
                EmpresaId = tenantProvider.EmpresaId.GetValueOrDefault()
            };

            try
            {
                await repositorioConfiguracaoPreco.InserirAsync(config);

                await contexto.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
            }

            registro = await repositorioConfiguracaoPreco.SelecionarPorIdAsync(config.Id);
        }

        var response = new SelecionarConfiguracaoPrecoResponse(
            registro.Gasolina,
            registro.Diesel,
            registro.Etanol
            );

        return Result.Ok(response);
    }
}
