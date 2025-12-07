using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarTodos;

public record SelecionarVeiculosDto(
    Guid Id,
    SelecionarGrupoVeiculoDtoSimplified GrupoVeiculo,
    string Placa,
    string Modelo,
    string Marca,
    string Cor,
    TipoCombustivel TipoCombustivel,
    decimal CapacidadeTanque,
    string? ImagemBase64
);

public record SelecionarVeiculosResponse
{
    public required int QuantidadeRegistros { get; init; }
    public required IEnumerable<SelecionarVeiculosDto> Registros { get; init; }
}
