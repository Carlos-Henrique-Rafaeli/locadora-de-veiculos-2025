using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.SelecionarPorId;

public record SelecionarTaxaServicoPorIdRequest(Guid Id)
    : IRequest<Result<SelecionarTaxaServicoPorIdResponse>>;
