using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloConfiguracao.Commands.Selecionar;

internal class SelecionarConfiguracaoPrecoRequestHandler(
    IRepositorioConfiguracaoPreco repositorioConfiguracaoPreco,
    ITenantProvider tenantProvider,
    IContextoPersistencia contexto
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
                UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault()
            };

            try
            {
                await repositorioConfiguracaoPreco.InserirAsync(config);

                await contexto.GravarAsync();
            }
            catch (Exception ex)
            {
                await contexto.RollbackAsync();

                return Result.Fail(ErrorResults.InternalServerError(ex));
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
