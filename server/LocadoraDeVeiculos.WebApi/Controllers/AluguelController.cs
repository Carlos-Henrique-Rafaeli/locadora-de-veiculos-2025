using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Inserir;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.SelecionarPorId;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.SelecionarTodos;
using LocadoraDeVeiculos.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/aluguel")]
public class AluguelController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(InserirAluguelResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Inserir(InserirAluguelRequest request)
    {
        var resultado = await mediator.Send(request);

        return resultado.ToHttpResponse();
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

        return resultado.ToHttpResponse();
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelecionarAlugueisResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var resultado = await mediator.Send(new SelecionarAlugueisRequest());

        return resultado.ToHttpResponse();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarAluguelPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarAluguelPorIdRequest(id);

        var resultado = await mediator.Send(selecionarPorIdRequest);

        return resultado.ToHttpResponse();
    }
}
