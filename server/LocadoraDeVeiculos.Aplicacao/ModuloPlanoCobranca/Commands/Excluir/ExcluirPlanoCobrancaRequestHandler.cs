using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Excluir;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Excluir;

internal class ExcluirPlanoCobrancaRequestHandler(
    IRepositorioPlanoCobranca repositorioPlanoCobranca,
    IContextoPersistencia contexto
) : IRequestHandler<ExcluirPlanoCobrancaRequest, Result<ExcluirPlanoCobrancaResponse>>
{
    public async Task<Result<ExcluirPlanoCobrancaResponse>> Handle(ExcluirPlanoCobrancaRequest request, CancellationToken cancellationToken)
    {
        var grupoVeiculoSelecionado = await repositorioPlanoCobranca.SelecionarPorIdAsync(request.Id);

        if (grupoVeiculoSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        try
        {
            await repositorioPlanoCobranca.ExcluirAsync(grupoVeiculoSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new ExcluirPlanoCobrancaResponse());
    }
}