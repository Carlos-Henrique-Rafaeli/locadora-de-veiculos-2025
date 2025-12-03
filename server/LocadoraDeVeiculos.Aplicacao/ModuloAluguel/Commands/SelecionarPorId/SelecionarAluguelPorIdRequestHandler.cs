using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.SelecionarTodos;
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
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        var resposta = new SelecionarAluguelPorIdResponse(
            new SelecionarAluguelDto(
                aluguelSelecionado.Id,
                    new SelecionarCondutoresDto(
                        aluguelSelecionado.Condutor.Id,
                        new SelecionarClienteDto(
                            aluguelSelecionado.Condutor.Cliente.Id,
                            aluguelSelecionado.Condutor.Cliente.TipoCliente,
                            aluguelSelecionado.Condutor.Cliente.Nome,
                            aluguelSelecionado.Condutor.Cliente.Telefone,
                            aluguelSelecionado.Condutor.Cliente.Cpf,
                            aluguelSelecionado.Condutor.Cliente.Cnpj,
                            aluguelSelecionado.Condutor.Cliente.Estado,
                            aluguelSelecionado.Condutor.Cliente.Cidade,
                            aluguelSelecionado.Condutor.Cliente.Bairro,
                            aluguelSelecionado.Condutor.Cliente.Rua,
                            aluguelSelecionado.Condutor.Cliente.Numero
                            ),
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