using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Excluir;

public class ExcluirGrupoVeiculosRequestHandler(
    IRepositorioGrupoVeiculos repositorioGrupoVeiculo,
    IRepositorioPlanoCobranca repositorioPlanoCobranca,
    IRepositorioAluguel repositorioAluguel,
    LocadoraDeVeiculosDbContext contexto
) : IRequestHandler<ExcluirGrupoVeiculosRequest, Result<ExcluirGrupoVeiculosResponse>>
{
    public async Task<Result<ExcluirGrupoVeiculosResponse>> Handle(ExcluirGrupoVeiculosRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var grupoVeiculoSelecionado = await repositorioGrupoVeiculo.SelecionarPorIdAsync(request.Id);

            if (grupoVeiculoSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            if (grupoVeiculoSelecionado.Veiculos.Any())
                return Result.Fail(GrupoVeiculosResultadosErro.GrupoVeiculoPossuiVeiculosErro());

            var planosCobrancas = await repositorioPlanoCobranca.SelecionarTodosAsync();

            if (planosCobrancas.Any(x => x.GrupoVeiculo.Id == request.Id))
                return Result.Fail(GrupoVeiculosResultadosErro.GrupoVeiculoPossuiPlanosErro());

            var alugueis = await repositorioAluguel.SelecionarTodosAsync();

            if (alugueis.Any(x => x.GrupoVeiculo.Id == grupoVeiculoSelecionado.Id))
                return Result.Fail(GrupoVeiculosResultadosErro.AluguelAtivoErro());

            await repositorioGrupoVeiculo.ExcluirAsync(request.Id);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new ExcluirGrupoVeiculosResponse());
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}