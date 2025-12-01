using FluentResults;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico;

public abstract class TaxaServicoErrorResults
{
    public static Error NomeDuplicadoError(string nome)
    {
        return new Error("Nome duplicado")
            .CausedBy($"Uma taxa/serviço com o nome '{nome}' já foi cadastrado.")
            .WithMetadata("ErrorType", "BadRequest");
    }
}
