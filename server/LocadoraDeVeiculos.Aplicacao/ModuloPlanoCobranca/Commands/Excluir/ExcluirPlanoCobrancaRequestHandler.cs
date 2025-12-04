using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Excluir;

internal class ExcluirPlanoCobrancaRequestHandler(
    IRepositorioPlanoCobranca repositorioPlanoCobranca,
    IRepositorioAluguel repositorioAluguel,
    LocadoraDeVeiculosDbContext contexto
) : IRequestHandler<ExcluirPlanoCobrancaRequest, Result<ExcluirPlanoCobrancaResponse>>
{
    public async Task<Result<ExcluirPlanoCobrancaResponse>> Handle(ExcluirPlanoCobrancaRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var planoCobrancaSelecionado = await repositorioPlanoCobranca.SelecionarPorIdAsync(request.Id);

            if (planoCobrancaSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            var alugueis = await repositorioAluguel.SelecionarTodosAsync();

            if (alugueis.Any(x => x.PlanoCobranca.Id == planoCobrancaSelecionado.Id))
                return Result.Fail(PlanoCobrancaResultadosErro.AluguelAtivoErro());

            await repositorioPlanoCobranca.ExcluirAsync(request.Id);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new ExcluirPlanoCobrancaResponse());
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}