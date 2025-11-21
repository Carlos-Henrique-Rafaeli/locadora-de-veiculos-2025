using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Excluir;

public record ExcluirVeiculoRequest(Guid Id) : IRequest<Result<ExcluirVeiculoResponse>>;
