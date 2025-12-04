using FluentResults;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos;

public abstract class GrupoVeiculosResultadosErro
{
    public static Error NomeDuplicadoErro(string nome)
    {
        return new Error("Nome duplicado")
            .CausedBy($"Um grupo de veículos com o nome '{nome}' já foi cadastrado.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error GrupoVeiculoPossuiVeiculosErro()
    {
        return new Error("Veículos associados")
            .CausedBy("Não é possível excluir um grupo de veículos que possui veículos associados.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error GrupoVeiculoPossuiPlanosErro()
    {
        return new Error("Planos de cobrança associados")
            .CausedBy("Não é possível excluir um grupo de veículos que possui planos de cobrança associados.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error AluguelAtivoErro()
    {
        return new Error("Aluguel ativo")
            .CausedBy($"O grupo de veículos pertence a um aluguel.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }
}
