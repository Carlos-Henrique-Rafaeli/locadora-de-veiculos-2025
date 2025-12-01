using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.SelecionarTodos;

internal class SelecionarTaxasServicosRequestHandler(
    IRepositorioTaxaServico repositorioTaxaServico
) : IRequestHandler<SelecionarTaxasServicosRequest, Result<SelecionarTaxasServicosResponse>>
{
    public async Task<Result<SelecionarTaxasServicosResponse>> Handle(
        SelecionarTaxasServicosRequest request, CancellationToken cancellationToken)
    {
        var registros = await repositorioTaxaServico.SelecionarTodosAsync();

        var response = new SelecionarTaxasServicosResponse
        {
            QuantidadeRegistros = registros.Count,
            Registros = registros
                .Select(r => new SelecionarTaxasServicosDto(
                    r.Id,
                    r.Nome,
                    r.Valor,
                    r.TipoCobranca
                    ))
        };

        return Result.Ok(response);
    }
}
