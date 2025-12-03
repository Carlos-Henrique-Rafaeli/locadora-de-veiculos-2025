using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Excluir;

internal class ExcluirAluguelRequestHandler(
    IRepositorioAluguel repositorioAluguel,
    IContextoPersistencia contexto
) : IRequestHandler<ExcluirAluguelRequest, Result<ExcluirAluguelResponse>>
{
    public async Task<Result<ExcluirAluguelResponse>> Handle(ExcluirAluguelRequest request, CancellationToken cancellationToken)
    {
        var aluguelSelecionado = await repositorioAluguel.SelecionarPorIdAsync(request.Id);

        if (aluguelSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        if (aluguelSelecionado.EstaAberto)
            return Result.Fail(AluguelErrorResults.AluguelAbertoError(aluguelSelecionado.Id));

        try
        {
            await repositorioAluguel.ExcluirAsync(aluguelSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new ExcluirAluguelResponse());
    }
}