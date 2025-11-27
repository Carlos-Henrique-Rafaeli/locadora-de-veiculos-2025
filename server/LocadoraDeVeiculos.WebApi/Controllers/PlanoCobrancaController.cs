using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Inserir;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.SelecionarPorId;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.SelecionarTodos;
using LocadoraDeVeiculos.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/plano-cobranca")]
public class PlanoCobrancaController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(InserirPlanoCobrancaResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Inserir(InserirPlanoCobrancaRequest request)
    {
        var resultado = await mediator.Send(request);

        return resultado.ToHttpResponse();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EditarPlanoCobrancaResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Editar(Guid id, EditarPlanoCobrancaPartialRequest request)
    {
        var editarRequest = new EditarPlanoCobrancaRequest(
            id,
            request.TipoPlano,
            request.GrupoVeiculoId,
            request.ValorDiario,
            request.ValorKm,
            request.KmIncluso,
            request.ValorKmExcedente,
            request.ValorFixo
        );

        var resultado = await mediator.Send(editarRequest);

        return resultado.ToHttpResponse();
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelecionarPlanosCobrancaResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var resultado = await mediator.Send(new SelecionarPlanosCobrancaRequest());

        return resultado.ToHttpResponse();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarPlanoCobrancaPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarPlanoCobrancaPorIdRequest(id);

        var resultado = await mediator.Send(selecionarPorIdRequest);

        return resultado.ToHttpResponse();
    }
}
