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
    //[HttpPost]
    //[ProducesResponseType(typeof(InserirPessoaFisicaResponse), StatusCodes.Status200OK)]
    //public async Task<IActionResult> Inserir(InserirPessoaFisicaRequest request)
    //{
    //    var resultado = await mediator.Send(request);

    //    return resultado.ToHttpResponse();
    //}


    //[HttpPut("{id:guid}")]
    //[ProducesResponseType(typeof(EditarPessoaFisicaResponse), StatusCodes.Status200OK)]
    //public async Task<IActionResult> Editar(Guid id, EditarPessoaFisicaPartialRequest request)
    //{
    //    var editarRequest = new EditarPessoaFisicaRequest(
    //        id,
    //        request.Nome,
    //        request.Telefone,
    //        request.Endereco,
    //        request.Cpf,
    //        request.Rg,
    //        request.Cnh,
    //        request.PessoaJuridicaId
    //    );

    //    var resultado = await mediator.Send(editarRequest);

    //    return resultado.ToHttpResponse();
    //}

    //[HttpDelete("{id:guid}")]
    //[ProducesResponseType(typeof(ExcluirPessoaFisicaResponse), StatusCodes.Status200OK)]
    //public async Task<IActionResult> Excluir(Guid id)
    //{
    //    var excluirRequest = new ExcluirPessoaFisicaRequest(id);

    //    var resultado = await mediator.Send(excluirRequest);

    //    return resultado.ToHttpResponse();
    //}

    //[HttpGet]
    //[ProducesResponseType(typeof(SelecionarPessoasFisicasResponse), StatusCodes.Status200OK)]
    //public async Task<IActionResult> SelecionarTodos()
    //{
    //    var resultado = await mediator.Send(new SelecionarPessoasFisicasRequest());

    //    return resultado.ToHttpResponse();
    //}

    //[HttpGet("{id:guid}")]
    //[ProducesResponseType(typeof(SelecionarPessoaFisicaPorIdResponse), StatusCodes.Status200OK)]
    //public async Task<IActionResult> SelecionarPorId(Guid id)
    //{
    //    var selecionarPorIdRequest = new SelecionarPessoaFisicaPorIdRequest(id);

    //    var resultado = await mediator.Send(selecionarPorIdRequest);

    //    return resultado.ToHttpResponse();
    //}
}
