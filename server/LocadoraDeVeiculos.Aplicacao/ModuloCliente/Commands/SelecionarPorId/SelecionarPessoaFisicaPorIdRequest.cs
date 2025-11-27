using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarPorId;

public record SelecionarPessoaFisicaPorIdRequest(Guid Id)
    : IRequest<Result<SelecionarPessoaFisicaPorIdResponse>>; 
