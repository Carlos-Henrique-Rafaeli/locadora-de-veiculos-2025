using FluentResults;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca;

public abstract class PlanoCobrancaErrorResults
{
    public static Error GrupoVeiculoNullError(Guid grupoId)
    {
        return new Error("Grupo inexistente")
            .CausedBy($"Um grupo com o Id '{grupoId}' não existe.")
            .WithMetadata("ErrorType", "BadRequest");
    }
}
