using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Editar;

public record EditarPlanoCobrancaPartialRequest(
    TipoPlano TipoPlano,
    Guid GrupoVeiculoId,
    decimal? ValorDiario,
    decimal? ValorKm,
    int? KmIncluso,
    decimal? ValorKmExcedente,
    decimal? ValorFixo
    );

public record EditarPlanoCobrancaRequest(
    Guid Id,
    TipoPlano TipoPlano,
    Guid GrupoVeiculoId,
    decimal? ValorDiario,
    decimal? ValorKm,
    int? KmIncluso,
    decimal? ValorKmExcedente,
    decimal? ValorFixo
    ) 
    : IRequest<Result<EditarPlanoCobrancaResponse>>;
