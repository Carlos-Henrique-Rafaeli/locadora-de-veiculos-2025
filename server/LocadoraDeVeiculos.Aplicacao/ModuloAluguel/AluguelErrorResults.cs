using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel;

public abstract class AluguelErrorResults
{
    public static Error CondutorNullError(Guid condutorId)
    {
        return new Error("Condutor inexistente")
            .CausedBy($"Um condutor com o Id '{condutorId}' não existe.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error GrupoVeiculoNullError(Guid grupoVeiculoId)
    {
        return new Error("Grupo de Veículos inexistente")
            .CausedBy($"Um grupo de veículos com o id '{grupoVeiculoId}' não existe.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error VeiculoNullError(Guid veiculoId)
    {
        return new Error("Veículo inexistente")
            .CausedBy($"Um veículo com o Id '{veiculoId}' não existe.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error PlanoCobrancaNullError(Guid planoCobrancaId)
    {
        return new Error("Plano de Cobrança inexistente")
            .CausedBy($"Um plano de cobrança com o Id '{planoCobrancaId}' não existe.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error TaxaServicoNullError(Guid taxaServicoId)
    {
        return new Error("Taxa ou Serviço inexistente")
            .CausedBy($"Uma taxa ou serviço com o Id '{taxaServicoId}' não existe.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error VeiculoNaoPertenceAoGrupoVeiculoError(string veiculo, string grupoVeiculo)
    {
        return new Error("Veículo não pertence ao Grupo de Veículos")
            .CausedBy($"O veículo '{veiculo}' não pertence ao grupo de veículos '{grupoVeiculo}'.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error PlanoCobrancaNaoPertenceAoGrupoVeiculoError(string planoCobranca, string grupoVeiculo)
    {
        return new Error("Plano de Cobrança não pertence ao Grupo de Veículos")
            .CausedBy($"O plano de cobrança '{planoCobranca}' não pertence ao grupo de veículos '{grupoVeiculo}'.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error AluguelFechadoError(Guid idAluguel)
    {
        return new Error("Aluguel Concluído")
            .CausedBy($"O aluguel com Id'{idAluguel}' já foi concluído.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error AluguelAbertoError(Guid idAluguel)
    {
        return new Error("Aluguel não Concluído")
            .CausedBy($"O aluguel com Id'{idAluguel}' não foi concluído.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error ValidadeCnhVencidaError(string cpf)
    {
        return new Error("Validade vencida")
            .CausedBy($"O condutor de Cpf '{cpf}' esta com a Cnh vencida.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error VeiculoJaSelecionadoError(string veiculo)
    {
        return new Error("Veículo pertence a outro aluguel")
            .CausedBy($"O veículo '{veiculo}' já pertence a um aluguel.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error KmRodadosError()
    {
        return new Error("KM rodados")
            .CausedBy($"O KM antigo tem que ser menor que o KM atual.")
            .WithMetadata("ErrorType", "BadRequest");
    }
    
    public static Error DataErradaError()
    {
        return new Error("Data Errada")
            .CausedBy($"A data de retorno precisa ser posterior à data de entrada.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error PorcentagemTanqueObrigatoriaError()
    {
        return new Error("Porcentagem Obrigatória")
            .CausedBy($"A porcentagem do tanque é obrigatória.")
            .WithMetadata("ErrorType", "BadRequest");
    }
}
