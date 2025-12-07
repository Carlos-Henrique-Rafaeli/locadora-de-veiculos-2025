using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Finalizar;

public record FinalizarAluguelPartialRequest(
    DateTime DataRetorno,
    decimal kmInicial,
    decimal kmAtual,
    bool tanqueCheio,
    decimal? porcentagemTanque
);

public record FinalizarAluguelRequest(
    Guid Id, 
    DateTime DataRetorno,
    decimal kmInicial,
    decimal kmAtual,
    bool tanqueCheio,
    decimal? porcentagemTanque
    ) 
    : IRequest<Result<FinalizarAluguelResponse>>;
