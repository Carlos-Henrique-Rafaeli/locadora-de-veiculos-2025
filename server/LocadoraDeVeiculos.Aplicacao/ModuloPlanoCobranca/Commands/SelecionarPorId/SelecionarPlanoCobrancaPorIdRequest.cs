using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.SelecionarPorId;

public record SelecionarPlanoCobrancaPorIdRequest(Guid Id) 
    : IRequest<Result<SelecionarPlanoCobrancaPorIdResponse>>;
