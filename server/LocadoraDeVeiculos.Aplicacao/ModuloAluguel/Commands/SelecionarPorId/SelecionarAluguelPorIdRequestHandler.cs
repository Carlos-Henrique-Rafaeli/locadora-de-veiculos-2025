using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.SelecionarPorId;

internal class SelecionarAluguelPorIdRequestHandler(
    IRepositorioAluguel repositorioAluguel
) : IRequestHandler<SelecionarAluguelPorIdRequest, Result<SelecionarAluguelPorIdResponse>>
{
    public async Task<Result<SelecionarAluguelPorIdResponse>> Handle(SelecionarAluguelPorIdRequest request, CancellationToken cancellationToken)
    {
        var aluguelSelecionado = await repositorioAluguel.SelecionarPorIdAsync(request.Id);

        if (aluguelSelecionado is null)
            return Result.Fail(ErrorResults.NotFoundError(request.Id));

        var resposta = new SelecionarAluguelPorIdResponse(
            new SelecionarAluguelDto(
                aluguelSelecionado.Id,
                    new SelecionarCondutorDto(
                        aluguelSelecionado.Condutor.Id,
                        aluguelSelecionado.Condutor.Nome,
                        aluguelSelecionado.Condutor.Email,
                        aluguelSelecionado.Condutor.Cpf,
                        aluguelSelecionado.Condutor.Cnh,
                        aluguelSelecionado.Condutor.ValidadeCnh,
                        aluguelSelecionado.Condutor.Telefone
                        ),
                    new SelecionarGrupoVeiculoDtoSimplified(
                        aluguelSelecionado.GrupoVeiculo.Id,
                        aluguelSelecionado.GrupoVeiculo.Nome,
                        aluguelSelecionado.GrupoVeiculo.Veiculos.Count()
                        ),
                    new SelecionarVeiculosDto(
                        aluguelSelecionado.Veiculo.Id,
                        aluguelSelecionado.Veiculo.GrupoVeiculo.Nome,
                        aluguelSelecionado.Veiculo.Placa,
                        aluguelSelecionado.Veiculo.Modelo,
                        aluguelSelecionado.Veiculo.Marca,
                        aluguelSelecionado.Veiculo.Cor,
                        aluguelSelecionado.Veiculo.TipoCombustivel,
                        aluguelSelecionado.Veiculo.CapacidadeTanque
                        ),
                    aluguelSelecionado.DataEntrada,
                    aluguelSelecionado.DataRetorno,
                    new SelecionarPlanoCobrancaDtoSimplified(
                        aluguelSelecionado.PlanoCobranca.Id,
                        aluguelSelecionado.PlanoCobranca.TipoPlano,
                        aluguelSelecionado.PlanoCobranca.GrupoVeiculo.Nome,
                        aluguelSelecionado.PlanoCobranca.ValorDiario,
                        aluguelSelecionado.PlanoCobranca.ValorKm,
                        aluguelSelecionado.PlanoCobranca.KmIncluso,
                        aluguelSelecionado.PlanoCobranca.ValorKmExcedente,
                        aluguelSelecionado.PlanoCobranca.ValorFixo
                        ),
                    aluguelSelecionado.EstaAberto ? aluguelSelecionado.CalcularValorTotal() : aluguelSelecionado.ValorFinal,
                    aluguelSelecionado.EstaAberto
                    )
            );

        return Result.Ok(resposta);
    }
}