using FluentResults;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo;

public abstract class VeiculoResultadosErro
{
    public static Error PlacaDuplicadaErro(string placa)
    {
        return new Error("Placa duplicada")
            .CausedBy($"Um veículo com a placa '{placa}' já foi cadastrado.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error GrupoVeiculoNullErro(Guid grupoId)
    {
        return new Error("Grupo inexistente")
            .CausedBy($"Um grupo com o Id '{grupoId}' não existe.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error AluguelAtivoErro()
    {
        return new Error("Aluguel ativo")
            .CausedBy($"O veículo pertence a um aluguel.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }
}
