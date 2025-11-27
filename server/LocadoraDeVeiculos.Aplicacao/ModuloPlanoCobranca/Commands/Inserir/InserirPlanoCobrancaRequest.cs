using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Inserir;

public record InserirPlanoCobrancaRequest(
    TipoPlano TipoPlano,
    Guid GrupoVeiculoId,
    decimal? ValorDiario,
    decimal? ValorKm,
    int? KmIncluso,
    decimal? ValorKmExcedente,
    decimal? ValorFixo
    ) 
    : IRequest<Result<InserirPlanoCobrancaResponse>>;
