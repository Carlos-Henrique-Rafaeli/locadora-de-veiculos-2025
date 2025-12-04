using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.SelecionarTodos;

internal class SelecionarFuncionariosRequestHandler(
    IRepositorioFuncionario repositorioFuncionario
) : IRequestHandler<SelecionarFuncionariosRequest, Result<SelecionarFuncionariosResponse>>
{
    public async Task<Result<SelecionarFuncionariosResponse>> Handle(
        SelecionarFuncionariosRequest query, CancellationToken cancellationToken)
    {
        var registros = await repositorioFuncionario.SelecionarTodosAsync();

        var response = new SelecionarFuncionariosResponse()
        {
            QuantidadeRegistros = registros.Count,
            Registros = registros
            .Select(x => new SelecionarFuncionariosDto(
                x.Id,
                x.NomeCompleto,
                x.Cpf,
                x.Email,
                x.Salario,
                x.AdmissaoEmUtc
                ))
        };
        return Result.Ok(response);
    }
}