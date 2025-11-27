using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Editar;

public record EditarPessoaJuridicaPartialRequest(
    string Nome,
    string Telefone,
    string Endereco,
    string Cnpj,
    Guid CondutorId
    );

public record EditarPessoaJuridicaRequest(
    Guid Id,
    string Nome,
    string Telefone,
    string Endereco,
    string Cnpj,
    Guid CondutorId
    ) 
    : IRequest<Result<EditarPessoaJuridicaResponse>>;


