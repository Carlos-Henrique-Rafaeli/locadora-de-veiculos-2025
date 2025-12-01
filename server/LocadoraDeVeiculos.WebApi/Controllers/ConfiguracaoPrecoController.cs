using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloConfiguracao.Commands.Editar;
using LocadoraDeVeiculos.Aplicacao.ModuloConfiguracao.Commands.Selecionar;
using LocadoraDeVeiculos.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/configuracoes")]
public class ConfiguracaoPrecoController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(SelecionarConfiguracaoPrecoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SelecionarTodos()
    {
        var resultado = await mediator.Send(new SelecionarConfiguracaoPrecoRequest());

        return resultado.ToHttpResponse();
    }

    [HttpPut]
    [ProducesResponseType(typeof(EditarConfiguracaoPrecoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Editar(EditarConfiguracaoPrecoRequest request)
    {
        var editarRequest = new EditarConfiguracaoPrecoRequest(
            request.Gasolina,
            request.Diesel,
            request.Etanol
        );

        var resultado = await mediator.Send(editarRequest);

        return resultado.ToHttpResponse();
    }
}
