using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Excluir;

internal class ExcluirPessoaFisicaRequestHandler(
    IRepositorioPessoaFisica repositorioPessoaFisica,
    IContextoPersistencia contexto
) : IRequestHandler<ExcluirPessoaFisicaRequest, Result<ExcluirPessoaFisicaResponse>>
{
    public async Task<Result<ExcluirPessoaFisicaResponse>> Handle(ExcluirPessoaFisicaRequest request, CancellationToken cancellationToken)
    {
        var condutorSelecionado = await repositorioPessoaFisica.SelecionarPorIdAsync(request.Id);

        if (condutorSelecionado is null)
            return Result.Fail(ErrorResults.NotFoundError(request.Id));

        try
        {
            await repositorioPessoaFisica.ExcluirAsync(condutorSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new ExcluirPessoaFisicaResponse());
    }
}