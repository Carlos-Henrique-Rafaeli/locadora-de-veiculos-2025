using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarPorId;

public record SelecionarVeiculoPorIdRequest(Guid Id) : IRequest<Result<SelecionarVeiculoPorIdResponse>>;
