using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.Excluir;

internal class ExcluirTaxaServicoRequestHandler(
    IRepositorioTaxaServico repositorioTaxaServico,
    IContextoPersistencia contexto
) : IRequestHandler<ExcluirTaxaServicoRequest, Result<ExcluirTaxaServicoResponse>>
{
    public async Task<Result<ExcluirTaxaServicoResponse>> Handle(ExcluirTaxaServicoRequest request, CancellationToken cancellationToken)
    {
        var grupoVeiculoSelecionado = await repositorioTaxaServico.SelecionarPorIdAsync(request.Id);

        if (grupoVeiculoSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        try
        {
            await repositorioTaxaServico.ExcluirAsync(grupoVeiculoSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new ExcluirTaxaServicoResponse());
    }
}