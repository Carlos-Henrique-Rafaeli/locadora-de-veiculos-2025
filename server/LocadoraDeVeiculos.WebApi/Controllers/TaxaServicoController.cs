using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.Inserir;
using LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.SelecionarPorId;
using LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.SelecionarTodos;
using LocadoraDeVeiculos.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/taxa-servico")]
public class TaxaServicoController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(InserirTaxaServicoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Inserir(InserirTaxaServicoRequest request)
    {
        var resultado = await mediator.Send(request);

        return resultado.ToHttpResponse();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EditarTaxaServicoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Editar(Guid id, EditarTaxaServicoPartialRequest request)
    {
        var editarRequest = new EditarTaxaServicoRequest(
            id,
            request.Nome,
            request.Valor,
            request.TipoCobranca
        );

        var resultado = await mediator.Send(editarRequest);

        return resultado.ToHttpResponse();
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelecionarTaxasServicosResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var resultado = await mediator.Send(new SelecionarTaxasServicosRequest());

        return resultado.ToHttpResponse();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarTaxaServicoPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarTaxaServicoPorIdRequest(id);

        var resultado = await mediator.Send(selecionarPorIdRequest);

        return resultado.ToHttpResponse();
    }
}
