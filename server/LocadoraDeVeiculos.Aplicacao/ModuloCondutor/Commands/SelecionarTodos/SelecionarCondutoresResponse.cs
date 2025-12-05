using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloCliente;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.SelecionarTodos;

public record SelecionarCondutoresDto(
    Guid Id,
    SelecionarClienteDtoSimplified Cliente,
    bool ClienteCondutor,
    string Nome,
    string Email,
    string Cpf,
    string Cnh,
    DateTime ValidadeCnh,
    string Telefone
    );

public record SelecionarClienteDtoSimplified(
    Guid Id,
    TipoCliente TipoCliente,
    string Nome,
    string Telefone,
    string? Cpf,
    string? Cnpj
    );

public record SelecionarCondutoresResponse
{
    public required int QuantidadeRegistros { get; init; }
    public required IEnumerable<SelecionarCondutoresDto> Registros { get; init; }
}