using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.SelecionarTodos;

internal class SelecionarAlugueisRequestHandler(
    IRepositorioAluguel repositorioAluguel
) : IRequestHandler<SelecionarAlugueisRequest, Result<SelecionarAlugueisResponse>>
{
    public async Task<Result<SelecionarAlugueisResponse>> Handle(
        SelecionarAlugueisRequest request, CancellationToken cancellationToken)
    {
        var registros = await repositorioAluguel.SelecionarTodosAsync();

        var response = new SelecionarAlugueisResponse
        {
            QuantidadeRegistros = registros.Count,
            Registros = registros
                .Select(r => new SelecionarAluguelDto(
                    r.Id,
                    new SelecionarCondutoresDto(
                        r.Condutor.Id,
                        new SelecionarClienteDtoSimplified(
                            r.Condutor.Cliente.Id,
                            r.Condutor.Cliente.TipoCliente,
                            r.Condutor.Cliente.Nome,
                            r.Condutor.Cliente.Telefone,
                            r.Condutor.Cliente.Cpf,
                            r.Condutor.Cliente.Cnpj
                            ),
                        r.Condutor.ClienteCondutor,
                        r.Condutor.Nome,
                        r.Condutor.Email,
                        r.Condutor.Cpf,
                        r.Condutor.Cnh,
                        r.Condutor.ValidadeCnh,
                        r.Condutor.Telefone
                        ),
                    new SelecionarGrupoVeiculoDtoSimplified(
                        r.GrupoVeiculo.Id,
                        r.GrupoVeiculo.Nome,
                        r.GrupoVeiculo.Veiculos.Count()
                        ),
                    new SelecionarVeiculosDto(
                        r.Veiculo.Id,
                        r.Veiculo.GrupoVeiculo.Nome,
                        r.Veiculo.Placa,
                        r.Veiculo.Modelo,
                        r.Veiculo.Marca,
                        r.Veiculo.Cor,
                        r.Veiculo.TipoCombustivel,
                        r.Veiculo.CapacidadeTanque
                        ),
                    r.DataEntrada,
                    r.DataRetorno,
                    new SelecionarPlanoCobrancaDtoSimplified(
                        r.PlanoCobranca.Id,
                        r.PlanoCobranca.TipoPlano,
                        r.PlanoCobranca.GrupoVeiculo.Nome,
                        r.PlanoCobranca.ValorDiario,
                        r.PlanoCobranca.ValorKm,
                        r.PlanoCobranca.KmIncluso,
                        r.PlanoCobranca.ValorKmExcedente,
                        r.PlanoCobranca.ValorFixo
                        ),
                    r.EstaAberto ? r.CalcularValorTotal() : r.ValorFinal,
                    r.EstaAberto))
        };

        return Result.Ok(response);
    }
}
