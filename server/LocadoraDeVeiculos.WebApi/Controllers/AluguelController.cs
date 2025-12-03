using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Excluir;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Finalizar;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Inserir;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.SelecionarPorId;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.SelecionarTodos;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize]
[Route("api/aluguel")]
public class AluguelController(IMediator mediator) : MainController
{
    [HttpPost]
    [ProducesResponseType(typeof(InserirAluguelResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Inserir(InserirAluguelRequest request)
    {
        var resultado = await mediator.Send(request);

        return ProcessarResultado(resultado);
    }

    [HttpPost("finalizar/{id:guid}")]
    [ProducesResponseType(typeof(FinalizarAluguelResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Finalizar(Guid id, FinalizarAluguelPartialRequest request)
    {
        var finalizarRequest = new FinalizarAluguelRequest(
            id,
            request.DataRetorno,
            request.kmInicial,
            request.kmAtual,
            request.tanqueCheio,
            request.porcentagemTanque
            );

        var resultado = await mediator.Send(finalizarRequest);

        return ProcessarResultado(resultado);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EditarAluguelResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Editar(Guid id, EditarAluguelPartialRequest request)
    {
        var editarRequest = new EditarAluguelRequest(
            id,
            request.CondutorId,
            request.GrupoVeiculoId,
            request.VeiculoId,
            request.DataEntrada,
            request.DataRetorno,
            request.PlanoCobrancaId,
            request.TaxasServicosIds
        );

        var resultado = await mediator.Send(editarRequest);

        return ProcessarResultado(resultado);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ExcluirAluguelResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var excluirRequest = new ExcluirAluguelRequest(id);

        var resultado = await mediator.Send(excluirRequest);

        return ProcessarResultado(resultado);
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelecionarAlugueisResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var resultado = await mediator.Send(new SelecionarAlugueisRequest());

        return ProcessarResultado(resultado);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarAluguelPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarAluguelPorIdRequest(id);

        var resultado = await mediator.Send(selecionarPorIdRequest);

        return ProcessarResultado(resultado);
    }
}
