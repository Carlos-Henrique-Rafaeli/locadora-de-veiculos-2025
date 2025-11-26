using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Inserir;

public record InserirPessoaJuridicaRequest(
    string Nome,
    string Telefone,
    string Endereco,
    string Cnpj,
    Guid CondutorId
    )
    : IRequest<Result<InserirPessoaJuridicaResponse>>;
