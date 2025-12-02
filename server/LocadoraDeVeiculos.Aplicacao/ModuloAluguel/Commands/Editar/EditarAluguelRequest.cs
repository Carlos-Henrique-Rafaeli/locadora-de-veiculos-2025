using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Editar;

public record EditarAluguelPartialRequest(
    Guid CondutorId,
    Guid GrupoVeiculoId,
    Guid VeiculoId,
    DateTime DataEntrada,
    DateTime DataRetorno,
    Guid PlanoCobrancaId,
    List<Guid> TaxasServicosIds
);

public record EditarAluguelRequest(
    Guid Id,
    Guid CondutorId,
    Guid GrupoVeiculoId,
    Guid VeiculoId,
    DateTime DataEntrada,
    DateTime DataRetorno,
    Guid PlanoCobrancaId,
    List<Guid> TaxasServicosIds
    ) 
    : IRequest<Result<EditarAluguelResponse>>;
