namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.SelecionarTodos;

public record SelecionarGrupoVeiculosDto(Guid id, string nome);

public record SelecionarGrupoVeiculosResponse 
{ 
    public required int QuantidadeRegistros { get; init; }
    public required IEnumerable<SelecionarGrupoVeiculosDto> Registros { get; init; }
}
