using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.SelecionarPorId;

internal class SelecionarTaxaServicoPorIdRequestHandler(
    IRepositorioTaxaServico repositorioTaxaServico
) : IRequestHandler<SelecionarTaxaServicoPorIdRequest, Result<SelecionarTaxaServicoPorIdResponse>>
{
    public async Task<Result<SelecionarTaxaServicoPorIdResponse>> Handle(SelecionarTaxaServicoPorIdRequest request, CancellationToken cancellationToken)
    {
        var taxaServicoSelecionada = await repositorioTaxaServico.SelecionarPorIdAsync(request.Id);

        if (taxaServicoSelecionada is null)
            return Result.Fail(ErrorResults.NotFoundError(request.Id));

        var resposta = new SelecionarTaxaServicoPorIdResponse(
            new SelecionarTaxasServicosDto(
                taxaServicoSelecionada.Id,
                taxaServicoSelecionada.Nome,
                taxaServicoSelecionada.Valor,
                taxaServicoSelecionada.TipoCobranca
            ));

        return Result.Ok(resposta);
    }
}