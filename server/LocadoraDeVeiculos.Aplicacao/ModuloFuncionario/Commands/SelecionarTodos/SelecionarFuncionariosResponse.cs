namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.SelecionarTodos;

public record SelecionarFuncionariosDto(
    Guid Id,
    string NomeCompleto,
    string Cpf,
    string Email,
    decimal Salario,
    DateTimeOffset AdmissaoEmUtc
);

public record SelecionarFuncionariosResponse
{
    public required int QuantidadeRegistros { get; init; }
    public required IEnumerable<SelecionarFuncionariosDto> Registros { get; init; }
}