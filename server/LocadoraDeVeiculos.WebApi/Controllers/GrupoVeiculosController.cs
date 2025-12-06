using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Excluir;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Inserir;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.SelecionarPorId;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.SelecionarTodos;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize]
[Route("api/grupo-veiculo")]
public class GrupoVeiculosController(IMediator mediator) : MainController
{
    [HttpPost]
    [ProducesResponseType(typeof(InserirGrupoVeiculosResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Inserir(InserirGrupoVeiculosRequest request)
    {
        var resultado = await mediator.Send(request);

        return ProcessarResultado(resultado);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EditarGrupoVeiculosResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Editar(Guid id, EditarGrupoVeiculosPartialRequest request)
    {
        var editarRequest = new EditarGrupoVeiculosRequest(
            id,
            request.Nome
        );

        var resultado = await mediator.Send(editarRequest);

        return ProcessarResultado(resultado);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ExcluirGrupoVeiculosResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var excluirRequest = new ExcluirGrupoVeiculosRequest(id);

        var resultado = await mediator.Send(excluirRequest);

        return ProcessarResultado(resultado);
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelecionarGrupoVeiculosResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var resultado = await mediator.Send(new SelecionarGrupoVeiculosRequest());

        return ProcessarResultado(resultado);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarGrupoVeiculoPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarGrupoVeiculoPorIdRequest(id);

        var resultado = await mediator.Send(selecionarPorIdRequest);

        return ProcessarResultado(resultado);
    }
}
