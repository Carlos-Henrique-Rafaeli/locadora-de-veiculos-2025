using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;

internal class SelecionarClientesRequestHandler(
    IRepositorioCliente repositorioCliente
) : IRequestHandler<SelecionarClientesRequest, Result<SelecionarClientesResponse>>
{
    public async Task<Result<SelecionarClientesResponse>> Handle(
        SelecionarClientesRequest request, CancellationToken cancellationToken)
    {
        var registros = await repositorioCliente.SelecionarTodosAsync();

        var response = new SelecionarClientesResponse
        {
            QuantidadeRegistros = registros.Count,
            Registros = registros
                .Select(x => new SelecionarClienteDto(
                    x.Id,
                    x.TipoCliente,
                    x.Nome,
                    x.Telefone,
                    x.Cpf,
                    x.Cnpj,
                    x.Estado,
                    x.Cidade,
                    x.Bairro,
                    x.Rua,
                    x.Numero
                    )
                )
        };

        return Result.Ok(response);
    }
}
