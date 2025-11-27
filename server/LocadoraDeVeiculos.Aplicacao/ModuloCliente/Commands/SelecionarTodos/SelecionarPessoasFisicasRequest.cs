using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;

public record SelecionarPessoasFisicasRequest() 
    : IRequest<Result<SelecionarPessoasFisicasResponse>>;



