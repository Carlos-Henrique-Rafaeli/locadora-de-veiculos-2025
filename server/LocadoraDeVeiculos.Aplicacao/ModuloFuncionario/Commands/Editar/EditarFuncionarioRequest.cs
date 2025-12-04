using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.Editar;

public record EditarFuncionarioPartialRequest(
    string NomeCompleto,
    string Cpf,
    decimal Salario,
    DateTimeOffset AdmissaoEmUtc
);

public record EditarFuncionarioRequest(
    Guid Id,
    string NomeCompleto,
    string Cpf,
    decimal Salario,
    DateTimeOffset AdmissaoEmUtc
) : IRequest<Result<EditarFuncionarioResponse>>;
