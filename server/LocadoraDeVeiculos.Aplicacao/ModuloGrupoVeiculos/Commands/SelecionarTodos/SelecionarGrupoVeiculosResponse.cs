using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.SelecionarTodos;

public record SelecionarGrupoVeiculosDto(Guid Id, string Nome, IEnumerable<SelecionarVeiculosGrupoVeiculosDto> Veiculos);

public record SelecionarVeiculosGrupoVeiculosDto(
    Guid Id,
    string Placa,
    string Modelo,
    string Marca,
    string Cor,
    TipoCombustivel TipoCombustivel,
    decimal CapacidadeTanque);

public record SelecionarGrupoVeiculosResponse 
{ 
    public required int QuantidadeRegistros { get; init; }
    public required IEnumerable<SelecionarGrupoVeiculosDto> Registros { get; init; }
}
