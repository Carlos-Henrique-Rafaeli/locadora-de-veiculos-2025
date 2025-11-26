using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Inserir;

public record InserirPessoaFisicaRequest(
    string Nome,
    string Telefone,
    string Endereco,
    string Cpf,
    string Rg,
    string Cnh,
    Guid? PessoaJuridicaId = null
    ) 
    : IRequest<Result<InserirPessoaFisicaResponse>>;
