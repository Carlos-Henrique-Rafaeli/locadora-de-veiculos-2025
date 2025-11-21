using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarTodos;

public record SelecionarVeiculosRequest : IRequest<Result<SelecionarVeiculosResponse>>;
