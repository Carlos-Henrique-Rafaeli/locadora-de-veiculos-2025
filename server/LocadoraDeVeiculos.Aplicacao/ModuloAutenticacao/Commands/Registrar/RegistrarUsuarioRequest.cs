using FluentResults;
using MediatR;
using LocadoraDeVeiculos.Aplicacao.ModuloAutenticacao.DTOs;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAutenticacao.Commands.Registrar;

public record RegistrarUsuarioRequest(string UserName, string Email, string Password) 
    : IRequest<Result<TokenResponse>>;