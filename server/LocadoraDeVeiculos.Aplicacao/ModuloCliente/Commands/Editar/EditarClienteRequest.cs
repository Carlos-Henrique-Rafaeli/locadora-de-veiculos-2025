using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Editar;

public record EditarClientePartialRequest(
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
    );

public record EditarClienteRequest(
    Guid Id,
    TipoCliente TipoCliente,
    string Nome,
    string Telefone,
    string? Cpf,
    string? Cnpj,
    TipoEstado Estado,
    string Cidade,
    string Bairro,
    string Rua,
    int Numero) : IRequest<Result<EditarClienteResponse>>;
