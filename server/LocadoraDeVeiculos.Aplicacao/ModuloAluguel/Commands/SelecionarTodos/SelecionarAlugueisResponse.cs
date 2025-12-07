using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.SelecionarTodos;

public record SelecionarAluguelDto(
    Guid Id,
    SelecionarCondutoresDto Condutor,
    SelecionarGrupoVeiculoDtoSimplified GrupoVeiculo,
    SelecionarVeiculosDto Veiculo,
    DateTime DataInicio,
    DateTime DataFim,
    SelecionarPlanoCobrancaDtoSimplified PlanoCobranca,
    List<SelecionarTaxasServicosDto> TaxasServicos,
    decimal ValorTotal,
    bool EstaAberta
);

public record SelecionarGrupoVeiculoDtoSimplified(
    Guid Id,
    string Nome
);

public record SelecionarPlanoCobrancaDtoSimplified(
    Guid Id,
    TipoPlano TipoPlano,
    SelecionarGrupoVeiculoDtoSimplified GrupoVeiculo,
    decimal? ValorDiario,
    decimal? ValorKm,
    int? KmIncluso,
    decimal? ValorKmExcedente,
    decimal? ValorFixo
);

public record SelecionarAlugueisResponse
{
    public required int QuantidadeRegistros { get; init; }
    public required IEnumerable<SelecionarAluguelDto> Registros { get; init; }
}
