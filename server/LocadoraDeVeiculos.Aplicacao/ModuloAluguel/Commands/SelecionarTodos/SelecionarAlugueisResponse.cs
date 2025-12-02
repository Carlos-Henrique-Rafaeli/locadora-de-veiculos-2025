using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.SelecionarTodos;

public record SelecionarAluguelDto(
    Guid Id,
    SelecionarCondutorDto Condutor,
    SelecionarGrupoVeiculoDtoSimplified GrupoVeiculos,
    SelecionarVeiculosDto Veiculo,
    DateTime DataInicio,
    DateTime DataFim,
    SelecionarPlanoCobrancaDtoSimplified PlanoCobranca,
    decimal ValorTotal
);

public record SelecionarGrupoVeiculoDtoSimplified(
    Guid Id,
    string Nome,
    int QtdVeiculos
);

public record SelecionarPlanoCobrancaDtoSimplified(
    Guid Id,
    TipoPlano TipoPlano,
    string NomeGrupoVeiculo,
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
