using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Inserir;
using LocadoraDeVeiculos.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/cliente")]
public class ClienteController(IMediator mediator) : ControllerBase
{
    [HttpPost("Pessoa-Fisica")]
    [ProducesResponseType(typeof(InserirPessoaFisicaResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> InserirPF(InserirPessoaFisicaRequest request)
    {
        var resultado = await mediator.Send(request);

        return resultado.ToHttpResponse();
    }

    [HttpPost("Pessoa-Juridica")]
    [ProducesResponseType(typeof(InserirPessoaJuridicaResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> InserirPJ(InserirPessoaJuridicaRequest request)
    {
        var resultado = await mediator.Send(request);

        return resultado.ToHttpResponse();
    }
}
