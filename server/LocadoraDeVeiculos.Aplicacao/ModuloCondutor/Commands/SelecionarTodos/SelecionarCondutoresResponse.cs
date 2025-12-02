using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.SelecionarTodos;

public record SelecionarCondutoresDto(
    Guid Id,
    SelecionarClienteDto Cliente,
    string Nome,
    string Email,
    string Cpf,
    string Cnh,
    DateTime ValidadeCnh,
    string Telefone
    );

public record SelecionarCondutoresResponse
{
    public required int QuantidadeRegistros { get; init; }
    public required IEnumerable<SelecionarCondutoresDto> Registros { get; init; }
}