using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.SelecionarTodos;

public record SelecionarPlanoCobrancaDto(
    Guid Id,
    TipoPlano TipoPlano,
    SelecionarGrupoVeiculoPlanoCobrancaDto GrupoVeiculo,
    decimal? ValorDiario,
    decimal? ValorKm,
    int? KmIncluso,
    decimal? ValorKmExcedente,
    decimal? ValorFixo
);

public record SelecionarGrupoVeiculoPlanoCobrancaDto(
    Guid Id,
    string Nome
);

public record SelecionarPlanosCobrancaResponse
{
    public required int QuantidadeRegistros { get; init; }
    public required IEnumerable<SelecionarPlanoCobrancaDto> Registros { get; init; }
}
