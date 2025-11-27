namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;

public record SelecionarPessoasJuridicasResponse
{
    public required int QuantidadeRegistros { get; init; }
    public required IEnumerable<SelecionarPessoaJuridicaDto> Registros { get; init; }
}
