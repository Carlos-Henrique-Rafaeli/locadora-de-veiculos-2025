using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.SelecionarPorId;

internal class SelecionarPlanoCobrancaPorIdRequestHandler(
    IRepositorioPlanoCobranca repositorioPlanoCobranca
) : IRequestHandler<SelecionarPlanoCobrancaPorIdRequest, Result<SelecionarPlanoCobrancaPorIdResponse>>
{
    public async Task<Result<SelecionarPlanoCobrancaPorIdResponse>> Handle(SelecionarPlanoCobrancaPorIdRequest request, CancellationToken cancellationToken)
    {
        var planoCobrancaSelecionado = await repositorioPlanoCobranca.SelecionarPorIdAsync(request.Id);

        if (planoCobrancaSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        var resposta = new SelecionarPlanoCobrancaPorIdResponse(
            new SelecionarPlanoCobrancaDto(
                planoCobrancaSelecionado.Id,
                planoCobrancaSelecionado.TipoPlano,
                new SelecionarGrupoVeiculoPlanoCobrancaDto(
                    planoCobrancaSelecionado.GrupoVeiculo.Id,
                    planoCobrancaSelecionado.GrupoVeiculo.Nome
                ),
                planoCobrancaSelecionado.ValorDiario,
                planoCobrancaSelecionado.ValorKm,
                planoCobrancaSelecionado.KmIncluso,
                planoCobrancaSelecionado.ValorKmExcedente,
                planoCobrancaSelecionado.ValorFixo
            ));

        return Result.Ok(resposta);
    }
}