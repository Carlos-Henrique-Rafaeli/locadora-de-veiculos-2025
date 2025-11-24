using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.Inserir;
using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.SelecionarPorId;
using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.SelecionarTodos;
using LocadoraDeVeiculos.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/condutor")]
public class CondutorController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(InserirCondutorResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Inserir(InserirCondutorRequest request)
    {
        var resultado = await mediator.Send(request);

        return resultado.ToHttpResponse();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EditarCondutorResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Editar(Guid id, EditarCondutorPartialRequest request)
    {
        var editarRequest = new EditarCondutorRequest(
            id,
            request.Nome,
            request.Email,
            request.Cpf,
            request.Cnh,
            request.ValidadeCnh,
            request.Telefone
        );

        var resultado = await mediator.Send(editarRequest);

        return resultado.ToHttpResponse();
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelecionarCondutoresResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var resultado = await mediator.Send(new SelecionarCondutoresRequest());

        return resultado.ToHttpResponse();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SelecionarCondutorPorIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        var selecionarPorIdRequest = new SelecionarCondutorPorIdRequest(id);

        var resultado = await mediator.Send(selecionarPorIdRequest);

        return resultado.ToHttpResponse();
    }
}
