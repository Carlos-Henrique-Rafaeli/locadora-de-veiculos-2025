using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.Inserir;

public record InserirFuncionarioRequest(
    string NomeCompleto,
    string Cpf,
    string Email,
    string Senha,
    string ConfirmarSenha,
    decimal Salario,
    DateTimeOffset AdmissaoEmUtc
) : IRequest<Result<InserirFuncionarioResponse>>;
