using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAutenticacao.Commands.Sair;

public record SairCommand(string RefreshTokenHash) : IRequest<Result>;