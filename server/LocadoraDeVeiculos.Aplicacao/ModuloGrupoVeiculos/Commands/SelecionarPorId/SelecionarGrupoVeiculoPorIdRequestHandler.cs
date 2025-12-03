using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.SelecionarPorId;

public class SelecionarGrupoVeiculoPorIdRequestHandler(
    IRepositorioGrupoVeiculos repositorioGrupoVeiculo
) : IRequestHandler<SelecionarGrupoVeiculoPorIdRequest, Result<SelecionarGrupoVeiculoPorIdResponse>>
{
    public async Task<Result<SelecionarGrupoVeiculoPorIdResponse>> Handle(SelecionarGrupoVeiculoPorIdRequest request, CancellationToken cancellationToken)
    {
        var grupoVeiculoSelecionado = await repositorioGrupoVeiculo.SelecionarPorIdAsync(request.Id);

        if (grupoVeiculoSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        var resposta = new SelecionarGrupoVeiculoPorIdResponse(
            grupoVeiculoSelecionado.Id,
            grupoVeiculoSelecionado.Nome,
            grupoVeiculoSelecionado.Veiculos
            .Select(x => new SelecionarVeiculosGrupoVeiculosDto(
                x.Id,
                x.Placa,
                x.Marca,
                x.Modelo,
                x.Cor,
                x.TipoCombustivel,
                x.CapacidadeTanque
                )
        ));

        return Result.Ok(resposta);
    }
}