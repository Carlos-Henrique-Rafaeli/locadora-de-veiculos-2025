using FluentResults;
using MediatR;
using LocadoraDeVeiculos.Aplicacao.ModuloAutenticacao.DTOs;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAutenticacao.Commands.Autenticar;

public record AutenticarUsuarioRequest(string UserName, string Password) : IRequest<Result<TokenResponse>>;