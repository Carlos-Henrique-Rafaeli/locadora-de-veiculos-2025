namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.SelecionarTodos;

public record SelecionarGrupoVeiculosDto(Guid Id, string Nome, IEnumerable<SelecionarVeiculosGrupoVeiculosDto> Veiculos);

public record SelecionarVeiculosGrupoVeiculosDto(
    Guid Id,
    string Placa
);

public record SelecionarGrupoVeiculosResponse 
{ 
    public required int QuantidadeRegistros { get; init; }
    public required IEnumerable<SelecionarGrupoVeiculosDto> Registros { get; init; }
}
