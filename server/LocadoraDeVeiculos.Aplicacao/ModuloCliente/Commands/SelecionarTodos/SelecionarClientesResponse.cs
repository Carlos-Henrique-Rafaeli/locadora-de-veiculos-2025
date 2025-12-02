using LocadoraDeVeiculos.Dominio.ModuloCliente;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;

public record SelecionarClienteDto(
    Guid Id,
    TipoCliente TipoCliente,
    string Nome,
    string Telefone,
    string? Cpf,
    string? Cnpj,
    TipoEstado Estado,
    string Cidade,
    string Bairro,
    string Rua,
    int Numero
);

public record SelecionarClientesResponse
{
    public required int QuantidadeRegistros { get; init; }
    public required IEnumerable<SelecionarClienteDto> Registros { get; init; }
}
