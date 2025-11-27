using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Excluir;

public record ExcluirPessoaFisicaRequest(Guid Id) 
    : IRequest<Result<ExcluirPessoaFisicaResponse>>;
