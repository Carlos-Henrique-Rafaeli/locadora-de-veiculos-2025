using FluentResults;
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
