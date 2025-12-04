using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Excluir;

internal class ExcluirClienteRequestHandler(
    IRepositorioCliente repositorioCliente,
    IRepositorioCondutor repositorioCondutor,
    LocadoraDeVeiculosDbContext contexto
) : IRequestHandler<ExcluirClienteRequest, Result<ExcluirClienteResponse>>
{
    public async Task<Result<ExcluirClienteResponse>> Handle(ExcluirClienteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var clienteSelecionado = await repositorioCliente.SelecionarPorIdAsync(request.Id);

            if (clienteSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            var condutores = await repositorioCondutor.SelecionarTodosAsync();

            if (condutores.Any(x => x.Cliente.Id == clienteSelecionado.Id))
                return Result.Fail(ClienteResultadosErro.ClienteEmCondutorErro(request.Id));

            await repositorioCliente.ExcluirAsync(request.Id);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new ExcluirClienteResponse());
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
