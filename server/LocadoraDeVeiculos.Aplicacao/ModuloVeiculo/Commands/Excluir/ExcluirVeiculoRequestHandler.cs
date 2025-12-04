using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Excluir;

public class ExcluirVeiculoRequestHandler(
    IRepositorioVeiculo repositorioVeiculo,
    IRepositorioGrupoVeiculos repositorioGrupoVeiculo,
    IRepositorioAluguel repositorioAluguel,
    LocadoraDeVeiculosDbContext contexto
) : IRequestHandler<ExcluirVeiculoRequest, Result<ExcluirVeiculoResponse>>
{
    public async Task<Result<ExcluirVeiculoResponse>> Handle(ExcluirVeiculoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var veiculoSelecionado = await repositorioVeiculo.SelecionarPorIdAsync(request.Id);

            if (veiculoSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            var grupoVeiculoSelecionado = await repositorioGrupoVeiculo.SelecionarPorIdAsync(veiculoSelecionado.GrupoVeiculo.Id);

            if (grupoVeiculoSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            var alugueis = await repositorioAluguel.SelecionarTodosAsync();

            if (alugueis.Any(x => x.Veiculo.Id == veiculoSelecionado.Id))
                return Result.Fail(VeiculoResultadosErro.AluguelAtivoErro());

            grupoVeiculoSelecionado.RemoverVeiculo(veiculoSelecionado);

            await repositorioVeiculo.ExcluirAsync(request.Id);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new ExcluirVeiculoResponse());
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}