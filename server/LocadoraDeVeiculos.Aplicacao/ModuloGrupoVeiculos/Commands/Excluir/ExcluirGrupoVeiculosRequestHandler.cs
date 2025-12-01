using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Excluir;

public class ExcluirGrupoVeiculosRequestHandler(
    IRepositorioGrupoVeiculos repositorioGrupoVeiculo,
    IRepositorioPlanoCobranca repositorioPlanoCobranca,
    IContextoPersistencia contexto
) : IRequestHandler<ExcluirGrupoVeiculosRequest, Result<ExcluirGrupoVeiculosResponse>>
{
    public async Task<Result<ExcluirGrupoVeiculosResponse>> Handle(ExcluirGrupoVeiculosRequest request, CancellationToken cancellationToken)
    {
        var grupoVeiculoSelecionado = await repositorioGrupoVeiculo.SelecionarPorIdAsync(request.Id);

        if (grupoVeiculoSelecionado is null)
            return Result.Fail(ErrorResults.NotFoundError(request.Id));

        if (grupoVeiculoSelecionado.Veiculos.Any())
            return Result.Fail(GrupoVeiculosErrorResults.GrupoVeiculoPossuiVeiculosError());

        var planosCobrancas = await repositorioPlanoCobranca.SelecionarTodosAsync();

        if (planosCobrancas.Any(x => x.GrupoVeiculo.Id == request.Id))
            return Result.Fail(GrupoVeiculosErrorResults.GrupoVeiculoPossuiPlanosError());


        try
        {
            await repositorioGrupoVeiculo.ExcluirAsync(grupoVeiculoSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new ExcluirGrupoVeiculosResponse());
    }
}