using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Inserir;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarPorId;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarTodos;
using LocadoraDeVeiculos.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/veiculos")]
public class VeiculoController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(InserirVeiculoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Inserir(InserirVeiculoRequest request)
    {
        var resultado = await mediator.Send(request);

        return resultado.ToHttpResponse();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EditarVeiculoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Editar(Guid id, EditarVeiculoPartialRequest request)
    {
        var editarRequest = new EditarVeiculoRequest(
            id,
            request.GrupoVeiculoId,
            request.Placa,
            request.Modelo,
            request.Marca,
            request.Cor,
            request.TipoCombustivel,
            request.CapacidadeTanque
        );

        var resultado = await mediator.Send(editarRequest);

        return resultado.ToHttpResponse();
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelecionarVeiculosResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var resultado = await mediator.Send(new SelecionarVeiculosRequest());

        return resultado.ToHttpResponse();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarVeiculoPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarVeiculoPorIdRequest(id);

        var resultado = await mediator.Send(selecionarPorIdRequest);

        return resultado.ToHttpResponse();
    }
}
