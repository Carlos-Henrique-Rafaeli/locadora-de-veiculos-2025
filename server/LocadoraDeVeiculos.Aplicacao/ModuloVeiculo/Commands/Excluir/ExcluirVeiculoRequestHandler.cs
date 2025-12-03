using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Excluir;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Excluir;

public class ExcluirVeiculoRequestHandler(
    IRepositorioVeiculo repositorioVeiculo,
    IRepositorioGrupoVeiculos repositorioGrupoVeiculo,
    IContextoPersistencia contexto
) : IRequestHandler<ExcluirVeiculoRequest, Result<ExcluirVeiculoResponse>>
{
    public async Task<Result<ExcluirVeiculoResponse>> Handle(ExcluirVeiculoRequest request, CancellationToken cancellationToken)
    {
        var veiculoSelecionado = await repositorioVeiculo.SelecionarPorIdAsync(request.Id);

        if (veiculoSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        var grupoVeiculoSelecionado = await repositorioGrupoVeiculo.SelecionarPorIdAsync(veiculoSelecionado.GrupoVeiculo.Id);

        if (grupoVeiculoSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        try
        {
            grupoVeiculoSelecionado.RemoverVeiculo(veiculoSelecionado);

            await repositorioVeiculo.ExcluirAsync(veiculoSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new ExcluirVeiculoResponse());
    }
}