namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;

public record SelecionarPessoaFisicaDto(
    Guid Id,
    string Nome,
    string Telefone,
    string Endereco,
    string Cpf,
    string Rg,
    string Cnh,
    SelecionarPessoaJuridicaDto? PessoaJuridica
    );

public record SelecionarPessoaJuridicaDto(
    Guid Id,
    string Nome,
    string Telefone,
    string Endereco,
    string Cnpj,
    SelecionarCondutorDto CondutorDto 
    );

public record SelecionarCondutorDto(
    Guid Id,
    string Nome,
    string Email,
    string Cpf,
    string Cnh,
    DateTime ValidadeCnh,
    string Telefone
    );

public record SelecionarPessoasFisicasResponse
{
    public required int QuantidadeRegistros { get; init; }
    public required IEnumerable<SelecionarPessoaFisicaDto> Registros { get; init; }
}
