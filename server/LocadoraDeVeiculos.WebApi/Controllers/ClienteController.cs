using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Inserir;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarPorId;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;
using LocadoraDeVeiculos.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/cliente")]
public class ClienteController(IMediator mediator) : ControllerBase
{
    [HttpPost("Pessoa-Fisica")]
    [ProducesResponseType(typeof(InserirPessoaFisicaResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> InserirPF(InserirPessoaFisicaRequest request)
    {
        var resultado = await mediator.Send(request);

        return resultado.ToHttpResponse();
    }

    [HttpPost("Pessoa-Juridica")]
    [ProducesResponseType(typeof(InserirPessoaJuridicaResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> InserirPJ(InserirPessoaJuridicaRequest request)
    {
        var resultado = await mediator.Send(request);

        return resultado.ToHttpResponse();
    }

    [HttpGet("Pessoa-Fisica")]
    [ProducesResponseType(typeof(SelecionarPessoasFisicasResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodosPf()
    {
        var resultado = await mediator.Send(new SelecionarPessoasFisicasRequest());

        return resultado.ToHttpResponse();
    }

    [HttpGet("Pessoa-Juridica")]
    [ProducesResponseType(typeof(SelecionarPessoasJuridicasResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodosPj()
    {
        var resultado = await mediator.Send(new SelecionarPessoasJuridicasRequest());

        return resultado.ToHttpResponse();
    }

    [HttpGet("Pessoa-Fisica/{id:guid}")]
    [ProducesResponseType(typeof(SelecionarPessoaFisicaPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorIdPf(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarPessoaFisicaPorIdRequest(id);

        var resultado = await mediator.Send(selecionarPorIdRequest);

        return resultado.ToHttpResponse();
    }

    [HttpGet("Pessoa-Juridica/{id:guid}")]
    [ProducesResponseType(typeof(SelecionarPessoaJuridicaPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorIdPj(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarPessoaJuridicaPorIdRequest(id);

        var resultado = await mediator.Send(selecionarPorIdRequest);

        return resultado.ToHttpResponse();
    }
}
