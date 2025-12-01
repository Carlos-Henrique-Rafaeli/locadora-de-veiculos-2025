using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.SelecionarTodos;

public record SelecionarTaxasServicosDto(
    Guid Id,
    string Nome,
    decimal Valor,
    TipoCobranca TipoCobranca
    );

public record SelecionarTaxasServicosResponse
{
    public required int QuantidadeRegistros { get; init; }
    public required IEnumerable<SelecionarTaxasServicosDto> Registros { get; init; }
}
