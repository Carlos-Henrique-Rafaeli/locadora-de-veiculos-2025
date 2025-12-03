using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Excluir;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Inserir;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.SelecionarPorId;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.SelecionarTodos;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize]
[Route("api/plano-cobranca")]
public class PlanoCobrancaController(IMediator mediator) : MainController
{
    [HttpPost]
    [ProducesResponseType(typeof(InserirPlanoCobrancaResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Inserir(InserirPlanoCobrancaRequest request)
    {
        var resultado = await mediator.Send(request);

        return ProcessarResultado(resultado);
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

        return ProcessarResultado(resultado);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ExcluirPlanoCobrancaResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var excluirRequest = new ExcluirPlanoCobrancaRequest(id);

        var resultado = await mediator.Send(excluirRequest);

        return ProcessarResultado(resultado);
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelecionarPlanosCobrancaResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var resultado = await mediator.Send(new SelecionarPlanosCobrancaRequest());

        return ProcessarResultado(resultado);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarPlanoCobrancaPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarPlanoCobrancaPorIdRequest(id);

        var resultado = await mediator.Send(selecionarPorIdRequest);

        return ProcessarResultado(resultado);
    }
}
