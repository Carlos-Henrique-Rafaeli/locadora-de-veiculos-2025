using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Excluir;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Excluir;

internal class ExcluirClienteRequestHandler(
    IRepositorioCliente repositorioCliente,
    IRepositorioCondutor repositorioCondutor,
    IContextoPersistencia contexto
) : IRequestHandler<ExcluirClienteRequest, Result<ExcluirClienteResponse>>
{
    public async Task<Result<ExcluirClienteResponse>> Handle(ExcluirClienteRequest request, CancellationToken cancellationToken)
    {
        var clienteSelecionado = await repositorioCliente.SelecionarPorIdAsync(request.Id);

        if (clienteSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        var condutores = await repositorioCondutor.SelecionarTodosAsync();

        if (condutores.Any(x => x.Cliente.Id == clienteSelecionado.Id))
            return Result.Fail(ClienteErrorResults.ClienteEmCondutorError(request.Id));

        try
        {
            await repositorioCliente.ExcluirAsync(clienteSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new ExcluirClienteResponse());
    }
}
