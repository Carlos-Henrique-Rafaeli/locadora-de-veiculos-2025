using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.SelecionarTodos;

public class SelecionarGrupoVeiculosRequestHandler(
    IRepositorioGrupoVeiculos repositorioGrupoVeiculos
) : IRequestHandler<SelecionarGrupoVeiculosRequest, Result<SelecionarGrupoVeiculosResponse>>
{
    public async Task<Result<SelecionarGrupoVeiculosResponse>> Handle(
        SelecionarGrupoVeiculosRequest request, CancellationToken cancellationToken)
    {
        var registros = await repositorioGrupoVeiculos.SelecionarTodosAsync();

        var response = new SelecionarGrupoVeiculosResponse
        {
            QuantidadeRegistros = registros.Count,
            Registros = registros
                .Select(r => new SelecionarGrupoVeiculosDto(r.Id, r.Nome, 
                r.Veiculos.Select(x => new SelecionarVeiculosGrupoVeiculosDto(
                    x.Id,
                    x.Placa,
                    x.Marca,
                    x.Modelo,
                    x.Cor,
                    x.TipoCombustivel,
                    x.CapacidadeTanque
                    ))
                ))
        };

        return Result.Ok(response);
    }
}
