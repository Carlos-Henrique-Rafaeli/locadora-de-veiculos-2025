using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.SelecionarPorId;

public record SelecionarGrupoVeiculoPorIdRequest(Guid Id) : IRequest<Result<SelecionarGrupoVeiculoPorIdResponse>>;

