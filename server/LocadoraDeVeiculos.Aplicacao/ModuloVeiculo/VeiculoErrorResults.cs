using FluentResults;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo;

public abstract class VeiculoErrorResults
{
    public static Error PlacaDuplicadaError(string placa)
    {
        return new Error("Placa duplicada")
            .CausedBy($"Um veículo com a placa '{placa}' já foi cadastrado.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error GrupoVeiculoNullError(Guid grupoId)
    {
        return new Error("Grupo inexistente")
            .CausedBy($"Um grupo com o Id '{grupoId}' não existe.")
            .WithMetadata("ErrorType", "BadRequest");
    }
}
