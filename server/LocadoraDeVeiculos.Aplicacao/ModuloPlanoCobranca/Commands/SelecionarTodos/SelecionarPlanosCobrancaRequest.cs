using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.SelecionarTodos;

public record SelecionarPlanosCobrancaRequest() 
    : IRequest<Result<SelecionarPlanosCobrancaResponse>>;
