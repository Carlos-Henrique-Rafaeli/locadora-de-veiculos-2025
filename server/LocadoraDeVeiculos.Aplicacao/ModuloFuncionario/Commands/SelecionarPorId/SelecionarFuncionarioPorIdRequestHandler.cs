using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.SelecionarPorId;

internal class SelecionarFuncionarioPorIdRequestHandler(IRepositorioFuncionario repositorioFuncionario)
    : IRequestHandler<SelecionarFuncionarioPorIdRequest, Result<SelecionarFuncionarioPorIdResponse>>
{
    public async Task<Result<SelecionarFuncionarioPorIdResponse>> Handle(
        SelecionarFuncionarioPorIdRequest query, CancellationToken cancellationToken)
    {
        var registroEncontrado = await repositorioFuncionario.SelecionarPorIdAsync(query.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

        var result = new SelecionarFuncionarioPorIdResponse(
            new SelecionarFuncionariosDto(
            registroEncontrado.Id,
            registroEncontrado.NomeCompleto,
            registroEncontrado.Cpf,
            registroEncontrado.Email,
            registroEncontrado.Salario,
            registroEncontrado.AdmissaoEmUtc
        ));

        return Result.Ok(result);
    }
}