using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Excluir;

internal class ExcluirPessoaJuridicaRequestHandler(
    IRepositorioPessoaJuridica repositorioPessoaJuridica,
    IRepositorioPessoaFisica repositorioPessoaFisica,
    IContextoPersistencia contexto
) : IRequestHandler<ExcluirPessoaJuridicaRequest, Result<ExcluirPessoaJuridicaResponse>>
{
    public async Task<Result<ExcluirPessoaJuridicaResponse>> Handle(ExcluirPessoaJuridicaRequest request, CancellationToken cancellationToken)
    {
        var condutorSelecionado = await repositorioPessoaJuridica.SelecionarPorIdAsync(request.Id);

        if (condutorSelecionado is null)
            return Result.Fail(ErrorResults.NotFoundError(request.Id));

        var pessoasFisicas = await repositorioPessoaFisica.SelecionarTodosAsync();

        if (pessoasFisicas.Any(x => x.PessoaJuridica?.Id == request.Id))
            return Result.Fail(ClienteErrorResults.PessoaFisicaVinculadaError());

        try
        {
            await repositorioPessoaJuridica.ExcluirAsync(condutorSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new ExcluirPessoaJuridicaResponse());
    }
}