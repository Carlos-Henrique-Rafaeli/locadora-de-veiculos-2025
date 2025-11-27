using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.SelecionarTodos;

internal class SelecionarPlanosCobrancaRequestHandler(
    IRepositorioPlanoCobranca repositorioPlanoCobranca
) : IRequestHandler<SelecionarPlanosCobrancaRequest, Result<SelecionarPlanosCobrancaResponse>>
{
    public async Task<Result<SelecionarPlanosCobrancaResponse>> Handle(
        SelecionarPlanosCobrancaRequest request, CancellationToken cancellationToken)
    {
        var registros = await repositorioPlanoCobranca.SelecionarTodosAsync();

        var response = new SelecionarPlanosCobrancaResponse
        {
            QuantidadeRegistros = registros.Count,
            Registros = registros
                .Select(r => new SelecionarPlanoCobrancaDto(
                    r.Id,
                    r.TipoPlano,
                    new SelecionarGrupoVeiculoPlanoCobrancaDto(
                        r.GrupoVeiculo.Id,
                        r.GrupoVeiculo.Nome
                    ),
                    r.ValorDiario,
                    r.ValorKm,
                    r.KmIncluso,
                    r.ValorKmExcedente,
                    r.ValorFixo))
        };

        return Result.Ok(response);
    }
}
