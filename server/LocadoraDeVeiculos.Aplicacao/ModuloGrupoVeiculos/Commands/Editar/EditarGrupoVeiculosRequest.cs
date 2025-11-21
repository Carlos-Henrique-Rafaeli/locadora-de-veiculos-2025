using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Editar;

public record EditarGrupoVeiculosPartialRequest(string Nome);

public record EditarGrupoVeiculosRequest(Guid Id, string Nome) 
    : IRequest<Result<EditarGrupoVeiculosResponse>>;
