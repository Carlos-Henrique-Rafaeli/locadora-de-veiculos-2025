using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarPorId;

public record SelecionarPessoaJuridicaPorIdRequest(Guid Id) 
    : IRequest<Result<SelecionarPessoaJuridicaPorIdResponse>>;
