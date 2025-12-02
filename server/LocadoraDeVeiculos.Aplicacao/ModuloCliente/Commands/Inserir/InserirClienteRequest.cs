using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Inserir;

public record InserirClienteRequest(
    TipoCliente TipoCliente,
    string Nome,
    string Telefone,
    string? Cpf,
    string? Cnpj,
    TipoEstado Estado,
    string Cidade,
    string Bairro,
    string Rua,
    int Numero
    ) : IRequest<Result<InserirClienteResponse>>;
