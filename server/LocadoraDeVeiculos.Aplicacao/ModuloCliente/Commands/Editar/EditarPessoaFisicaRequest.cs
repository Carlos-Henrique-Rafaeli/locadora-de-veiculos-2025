using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Editar;

public record EditarPessoaFisicaPartialRequest(
    string Nome,
    string Telefone,
    string Endereco,
    string Cpf,
    string Rg,
    string Cnh,
    Guid? PessoaJuridicaId = null
    );

public record EditarPessoaFisicaRequest(
    Guid Id,
    string Nome,
    string Telefone,
    string Endereco,
    string Cpf,
    string Rg,
    string Cnh,
    Guid? PessoaJuridicaId = null
    )
    : IRequest<Result<EditarPessoaFisicaResponse>>;
