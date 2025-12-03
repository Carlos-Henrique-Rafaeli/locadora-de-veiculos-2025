using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Excluir;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Inserir;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarPorId;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize]
[Route("api/cliente")]
public class ClienteController(IMediator mediator) : MainController
{
    [HttpPost]
    [ProducesResponseType(typeof(InserirClienteResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Inserir(InserirClienteRequest request)
    {
        var resultado = await mediator.Send(request);

        return ProcessarResultado(resultado);
    }


    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EditarClienteResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Editar(Guid id, EditarClientePartialRequest request)
    {
        var editarRequest = new EditarClienteRequest(
            id,
            request.TipoCliente,
            request.Nome,
            request.Telefone,
            request.Cpf,
            request.Cnpj,
            request.Estado,
            request.Cidade,
            request.Bairro,
            request.Rua,
            request.Numero
        );

        var resultado = await mediator.Send(editarRequest);

        return ProcessarResultado(resultado);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ExcluirClienteResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var excluirRequest = new ExcluirClienteRequest(id);

        var resultado = await mediator.Send(excluirRequest);

        return ProcessarResultado(resultado);
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelecionarClientesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var resultado = await mediator.Send(new SelecionarClientesRequest());

        return ProcessarResultado(resultado);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarClientePorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarClientePorIdRequest(id);

        var resultado = await mediator.Send(selecionarPorIdRequest);

        return ProcessarResultado(resultado);
    }
}
