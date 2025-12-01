using FluentResults;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos;

public abstract class GrupoVeiculosErrorResults
{
    public static Error NomeDuplicadoError(string nome)
    {
        return new Error("Nome duplicado")
            .CausedBy($"Um grupo de veículos com o nome '{nome}' já foi cadastrado.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error GrupoVeiculoPossuiVeiculosError()
    {
        return new Error("Veículos associados")
            .CausedBy("Não é possível excluir um grupo de veículos que possui veículos associados.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error GrupoVeiculoPossuiPlanosError()
    {
        return new Error("Planos de cobrança associados")
            .CausedBy("Não é possível excluir um grupo de veículos que possui planos de cobrança associados.")
            .WithMetadata("ErrorType", "BadRequest");
    }
}
