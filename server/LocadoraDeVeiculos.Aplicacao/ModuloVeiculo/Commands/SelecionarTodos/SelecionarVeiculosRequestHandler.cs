using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarTodos;

public class SelecionarVeiculosRequestHandler(
    IRepositorioVeiculo repositorioVeiculo
) : IRequestHandler<SelecionarVeiculosRequest, Result<SelecionarVeiculosResponse>>
{
    public async Task<Result<SelecionarVeiculosResponse>> Handle(
        SelecionarVeiculosRequest request, CancellationToken cancellationToken)
    {
        var registros = await repositorioVeiculo.SelecionarTodosAsync();

        var response = new SelecionarVeiculosResponse
        {
            QuantidadeRegistros = registros.Count,
            Registros = registros
                .Select(x => new SelecionarVeiculosDto(
                    x.Id,
                    new SelecionarGrupoVeiculoDtoSimplified(
                        x.GrupoVeiculo.Id,
                        x.GrupoVeiculo.Nome
                        ),
                    x.Placa,
                    x.Marca,
                    x.Modelo,
                    x.Cor,
                    x.TipoCombustivel,
                    x.CapacidadeTanque
                    )
                )
        };

        return Result.Ok(response);
    }
}