using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;

public class SelecionarClientesRequest() : IRequest<Result<SelecionarClientesResponse>>;
