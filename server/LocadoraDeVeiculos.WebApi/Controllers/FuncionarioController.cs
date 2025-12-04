using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.Excluir;
using LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.Inserir;
using LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.SelecionarPorId;
using LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.SelecionarTodos;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize(Roles = "Empresa")]
[Route("api/funcionarios")]
public sealed class FuncionarioController(IMediator mediator) : MainController
{
    [HttpPost]
    public async Task<ActionResult<InserirFuncionarioResponse>> Inserir(
        InserirFuncionarioRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await mediator.Send(request, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            var response = new InserirFuncionarioResponse(valor.Id);

            return CreatedAtAction(nameof(SelecionarPorId), new { id = valor.Id }, response);
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EditarFuncionarioResponse>> Editar(
       Guid id,
       EditarFuncionarioPartialRequest request,
       CancellationToken cancellationToken
    )
    {
        var command = new EditarFuncionarioRequest(
            id,
            request.NomeCompleto,
            request.Cpf,
            request.Salario,
            request.AdmissaoEmUtc
        );

        var result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            var response = new EditarFuncionarioResponse(valor.Id);

            return Ok(response);
        });
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ExcluirFuncionarioResponse>> Excluir(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var command = new ExcluirFuncionarioRequest(id);

        var result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (_) => NoContent());
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarFuncionariosResponse>> SelecionarTodos(
        CancellationToken cancellationToken
    )
    {
        var query = new SelecionarFuncionariosRequest();

        var result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            var response = new SelecionarFuncionariosResponse()
            {
                QuantidadeRegistros = valor.QuantidadeRegistros,
                Registros = valor.Registros
            };
            return Ok(response);
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelecionarFuncionarioPorIdResponse>> SelecionarPorId(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var query = new SelecionarFuncionarioPorIdRequest(id);

        var result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            var response = new SelecionarFuncionarioPorIdResponse(
                new SelecionarFuncionariosDto(
                valor.Funcionario.Id,
                valor.Funcionario.NomeCompleto,
                valor.Funcionario.Cpf,
                valor.Funcionario.Email,
                valor.Funcionario.Salario,
                valor.Funcionario.AdmissaoEmUtc
            ));

            return Ok(response);
        });
    }
}
