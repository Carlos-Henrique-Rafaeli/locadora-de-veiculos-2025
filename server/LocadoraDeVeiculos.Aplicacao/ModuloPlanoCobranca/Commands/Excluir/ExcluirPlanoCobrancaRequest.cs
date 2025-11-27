using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Excluir;

public record ExcluirPlanoCobrancaRequest(Guid Id) 
    : IRequest<Result<ExcluirPlanoCobrancaResponse>>;
