using FluentResults;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico;

public abstract class TaxaServicoResultadosErro
{
    public static Error NomeDuplicadoErro(string nome)
    {
        return new Error("Nome duplicado")
            .CausedBy($"Uma taxa/serviço com o nome '{nome}' já foi cadastrado.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error AluguelAtivoErro()
    {
        return new Error("Aluguel ativo")
            .CausedBy($"A taxa/serviço pertence a um aluguel.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }
}
