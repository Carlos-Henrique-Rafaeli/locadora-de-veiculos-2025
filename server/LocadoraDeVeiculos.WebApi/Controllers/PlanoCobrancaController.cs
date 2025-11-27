using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Inserir;
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
}
