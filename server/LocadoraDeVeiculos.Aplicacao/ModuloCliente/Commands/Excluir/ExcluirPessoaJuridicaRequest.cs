using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Excluir;

public record ExcluirPessoaJuridicaRequest(Guid Id) 
    : IRequest<Result<ExcluirPessoaJuridicaResponse>>;
