using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarPorId;

internal class SelecionarClientePorIdRequestHandler(
    IRepositorioCliente repositorioCliente
) : IRequestHandler<SelecionarClientePorIdRequest, Result<SelecionarClientePorIdResponse>>
{
    public async Task<Result<SelecionarClientePorIdResponse>> Handle(SelecionarClientePorIdRequest request, CancellationToken cancellationToken)
    {
        var clienteSelecionado = await repositorioCliente.SelecionarPorIdAsync(request.Id);

        if (clienteSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        var resposta = new SelecionarClientePorIdResponse(
            new SelecionarClienteDto(
                clienteSelecionado.Id,
                clienteSelecionado.TipoCliente,
                clienteSelecionado.Nome,
                clienteSelecionado.Telefone,
                clienteSelecionado.Cpf,
                clienteSelecionado.Cnpj,
                clienteSelecionado.Estado,
                clienteSelecionado.Cidade,
                clienteSelecionado.Bairro,
                clienteSelecionado.Rua,
                clienteSelecionado.Numero
                ));

        return Result.Ok(resposta);
    }
}