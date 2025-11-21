using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.SelecionarTodos;

public record SelecionarGrupoVeiculosRequest : IRequest<Result<SelecionarGrupoVeiculosResponse>>;

