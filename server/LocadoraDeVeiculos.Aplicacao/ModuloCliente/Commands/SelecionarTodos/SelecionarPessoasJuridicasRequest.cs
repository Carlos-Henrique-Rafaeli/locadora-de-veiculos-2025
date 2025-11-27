using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;

public record SelecionarPessoasJuridicasRequest() 
    : IRequest<Result<SelecionarPessoasJuridicasResponse>>;
