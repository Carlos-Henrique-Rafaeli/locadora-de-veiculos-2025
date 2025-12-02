using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Finalizar;

public record FinalizarAluguelPartialRequest(
    DateTime DataRetorno,
    int kmInicial,
    int kmAtual,
    bool tanqueCheio,
    decimal? porcentagemTanque
);

public record FinalizarAluguelRequest(
    Guid Id, 
    DateTime DataRetorno,
    int kmInicial,
    int kmAtual,
    bool tanqueCheio,
    decimal? porcentagemTanque
    ) 
    : IRequest<Result<FinalizarAluguelResponse>>;
