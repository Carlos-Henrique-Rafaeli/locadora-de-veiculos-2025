using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Inserir;

public record InserirAluguelRequest(
    Guid CondutorId,
    Guid GrupoVeiculoId,
    Guid VeiculoId,
    DateTime DataEntrada,
    DateTime DataRetorno,
    Guid PlanoCobrancaId,
    List<Guid> TaxasServicosIds
    )
    : IRequest<Result<InserirAluguelResponse>>;
