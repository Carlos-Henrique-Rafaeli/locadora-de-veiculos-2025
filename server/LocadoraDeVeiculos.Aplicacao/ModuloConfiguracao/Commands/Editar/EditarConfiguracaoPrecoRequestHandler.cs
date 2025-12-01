using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloConfiguracao.Commands.Editar;

internal class EditarConfiguracaoPrecoRequestHandler(
    IRepositorioConfiguracaoPreco repositorioConfiguracaoPreco,
    IContextoPersistencia contexto,
    ITenantProvider tenantProvider,
    IValidator<ConfiguracaoPreco> validador
) : IRequestHandler<EditarConfiguracaoPrecoRequest, Result<EditarConfiguracaoPrecoResponse>>
{
    public async Task<Result<EditarConfiguracaoPrecoResponse>> Handle(EditarConfiguracaoPrecoRequest request, CancellationToken cancellationToken)
    {
        var configuracoesPrecosSelecionadas = await repositorioConfiguracaoPreco.SelecionarTodosAsync();

        var configuracaoPrecoSelecionada = configuracoesPrecosSelecionadas.FirstOrDefault();

        if (configuracaoPrecoSelecionada == null)
        {
            var config = new ConfiguracaoPreco
            {
                Gasolina = request.Gasolina,
                Diesel = request.Diesel,
                Etanol = request.Etanol,
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

            configuracaoPrecoSelecionada = await repositorioConfiguracaoPreco.SelecionarPorIdAsync(config.Id);
        }

        configuracaoPrecoSelecionada.Gasolina = request.Gasolina;
        configuracaoPrecoSelecionada.Diesel = request.Diesel;
        configuracaoPrecoSelecionada.Etanol= request.Etanol;

        var resultadoValidacao =
            await validador.ValidateAsync(configuracaoPrecoSelecionada, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ErrorResults.BadRequestError(erros));
        }

        try
        {
            await repositorioConfiguracaoPreco.EditarAsync(configuracaoPrecoSelecionada);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new EditarConfiguracaoPrecoResponse());
    }
}
