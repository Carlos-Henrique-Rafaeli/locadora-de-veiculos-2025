using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Inserir;
using LocadoraDeVeiculos.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/grupo_veiculos")]
public class GrupoVeiculosController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(InserirGrupoVeiculosResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Inserir(InserirGrupoVeiculosRequest request)
    {
        var resultado = await mediator.Send(request);

        return resultado.ToHttpResponse();
    }
}
