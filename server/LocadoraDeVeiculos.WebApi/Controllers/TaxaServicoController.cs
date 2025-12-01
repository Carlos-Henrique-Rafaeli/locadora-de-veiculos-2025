using LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.Inserir;
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
}
