using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.Excluir;

internal class ExcluirTaxaServicoRequestHandler(
    IRepositorioTaxaServico repositorioTaxaServico,
    IRepositorioAluguel repositorioAluguel,
    LocadoraDeVeiculosDbContext contexto
) : IRequestHandler<ExcluirTaxaServicoRequest, Result<ExcluirTaxaServicoResponse>>
{
    public async Task<Result<ExcluirTaxaServicoResponse>> Handle(ExcluirTaxaServicoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var taxaServicoSelecionado = await repositorioTaxaServico.SelecionarPorIdAsync(request.Id);

            if (taxaServicoSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            var alugueis = await repositorioAluguel.SelecionarTodosAsync();

            if (alugueis.Any(x => x.TaxasServicos.Any(y => y.Id == taxaServicoSelecionado.Id)))
                return Result.Fail(TaxaServicoResultadosErro.AluguelAtivoErro());

            await repositorioTaxaServico.ExcluirAsync(request.Id);

            await contexto.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new ExcluirTaxaServicoResponse());
    }
}