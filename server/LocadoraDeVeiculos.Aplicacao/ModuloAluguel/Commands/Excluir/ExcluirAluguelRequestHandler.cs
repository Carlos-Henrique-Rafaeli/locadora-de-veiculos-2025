using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Excluir;

internal class ExcluirAluguelRequestHandler(
    IRepositorioAluguel repositorioAluguel,
    LocadoraDeVeiculosDbContext contexto
) : IRequestHandler<ExcluirAluguelRequest, Result<ExcluirAluguelResponse>>
{
    public async Task<Result<ExcluirAluguelResponse>> Handle(ExcluirAluguelRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var aluguelSelecionado = await repositorioAluguel.SelecionarPorIdAsync(request.Id);

            if (aluguelSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            if (aluguelSelecionado.EstaAberto)
                return Result.Fail(AluguelResultadosErro.AluguelAbertoErro(aluguelSelecionado.Id));

            await repositorioAluguel.ExcluirAsync(aluguelSelecionado.Id);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new ExcluirAluguelResponse());

        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}