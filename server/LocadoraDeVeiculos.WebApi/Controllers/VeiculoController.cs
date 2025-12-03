using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Excluir;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Inserir;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarPorId;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarTodos;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize]
[Route("api/veiculos")]
public class VeiculoController(IMediator mediator) : MainController
{
    [HttpPost]
    [ProducesResponseType(typeof(InserirVeiculoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Inserir(InserirVeiculoRequest request)
    {
        var resultado = await mediator.Send(request);

        return ProcessarResultado(resultado);
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

        return ProcessarResultado(resultado);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ExcluirVeiculoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var excluirRequest = new ExcluirVeiculoRequest(id);

        var resultado = await mediator.Send(excluirRequest);

        return ProcessarResultado(resultado);
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelecionarVeiculosResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var resultado = await mediator.Send(new SelecionarVeiculosRequest());

        return ProcessarResultado(resultado);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarVeiculoPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarVeiculoPorIdRequest(id);

        var resultado = await mediator.Send(selecionarPorIdRequest);

        return ProcessarResultado(resultado);
    }
}
