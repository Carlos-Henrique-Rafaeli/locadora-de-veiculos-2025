using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;


namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.Excluir;

internal class ExcluirCondutorRequestHandler(
    IRepositorioCondutor repositorioCondutor,
    IRepositorioAluguel repositorioAluguel,
    LocadoraDeVeiculosDbContext contexto
) : IRequestHandler<ExcluirCondutorRequest, Result<ExcluirCondutorResponse>>
{
    public async Task<Result<ExcluirCondutorResponse>> Handle(ExcluirCondutorRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var condutorSelecionado = await repositorioCondutor.SelecionarPorIdAsync(request.Id);

            if (condutorSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            var alugueis = await repositorioAluguel.SelecionarTodosAsync();

            if (alugueis.Any(x => x.Condutor.Id == condutorSelecionado.Id))
                return Result.Fail(CondutorResultadosErro.AluguelAtivoErro());

            await repositorioCondutor.ExcluirAsync(request.Id);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new ExcluirCondutorResponse());
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
