using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarPorId;

internal class SelecionarVeiculoPorIdRequestHandler(
    IRepositorioVeiculo repositorioVeiculo
) : IRequestHandler<SelecionarVeiculoPorIdRequest, Result<SelecionarVeiculoPorIdResponse>>
{
    public async Task<Result<SelecionarVeiculoPorIdResponse>> Handle(SelecionarVeiculoPorIdRequest request, CancellationToken cancellationToken)
    {
        var veiculoSelecionado = await repositorioVeiculo.SelecionarPorIdAsync(request.Id);

        if (veiculoSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        var resposta = new SelecionarVeiculoPorIdResponse(
            new SelecionarVeiculosDto(
                veiculoSelecionado.Id,
                veiculoSelecionado.GrupoVeiculo.Nome,
                veiculoSelecionado.Placa,
                veiculoSelecionado.Modelo,
                veiculoSelecionado.Marca,
                veiculoSelecionado.Cor,
                veiculoSelecionado.TipoCombustivel,
                veiculoSelecionado.CapacidadeTanque
                ));

        return Result.Ok(resposta);
    }
}
