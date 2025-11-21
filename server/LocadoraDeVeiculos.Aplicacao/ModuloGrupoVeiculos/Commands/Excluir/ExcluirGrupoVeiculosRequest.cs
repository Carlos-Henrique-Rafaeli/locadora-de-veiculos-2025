using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Excluir;

public record ExcluirGrupoVeiculosRequest(Guid Id) : IRequest<Result<ExcluirGrupoVeiculosResponse>>;
