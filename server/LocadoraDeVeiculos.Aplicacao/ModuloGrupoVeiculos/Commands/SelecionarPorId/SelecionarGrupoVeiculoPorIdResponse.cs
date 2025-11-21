using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.SelecionarTodos;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.SelecionarPorId;

public record SelecionarGrupoVeiculoPorIdResponse(Guid Id, string Nome, IEnumerable<SelecionarVeiculosGrupoVeiculosDto> Veiculos);
