using FluentResults;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca;

public abstract class PlanoCobrancaResultadosErro
{
    public static Error GrupoVeiculoNullErro(Guid grupoId)
    {
        return new Error("Grupo inexistente")
            .CausedBy($"Um grupo com o Id '{grupoId}' não existe.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error AluguelAtivoErro()
    {
        return new Error("Aluguel ativo")
            .CausedBy($"O plano de cobrança pertence a um aluguel.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }
}
