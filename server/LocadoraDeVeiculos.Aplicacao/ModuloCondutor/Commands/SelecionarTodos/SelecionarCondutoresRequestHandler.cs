using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.SelecionarTodos;

internal class SelecionarCondutoresRequestHandler(
    IRepositorioCondutor repositorioCondutor
) : IRequestHandler<SelecionarCondutoresRequest, Result<SelecionarCondutoresResponse>>
{
    public async Task<Result<SelecionarCondutoresResponse>> Handle(
        SelecionarCondutoresRequest request, CancellationToken cancellationToken)
    {
        var registros = await repositorioCondutor.SelecionarTodosAsync();

        var response = new SelecionarCondutoresResponse
        {
            QuantidadeRegistros = registros.Count,
            Registros = registros
                .Select(x => new SelecionarCondutoresDto(
                    x.Id,
                    new SelecionarClienteDtoSimplified(
                        x.Cliente.Id,
                        x.Cliente.TipoCliente,
                        x.Cliente.Nome,
                        x.Cliente.Telefone,
                        x.Cliente.Cpf,
                        x.Cliente.Cnpj
                        ),
                    x.ClienteCondutor,
                    x.Nome,
                    x.Email,
                    x.Cpf,
                    x.Cnh,
                    x.ValidadeCnh,
                    x.Telefone
                    )
                )
        };

        return Result.Ok(response);
    }
}
