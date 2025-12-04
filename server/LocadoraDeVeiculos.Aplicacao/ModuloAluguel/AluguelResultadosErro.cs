using FluentResults;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel;

public abstract class AluguelResultadosErro
{
    public static Error CondutorNullErro(Guid condutorId)
    {
        return new Error("Condutor inexistente")
            .CausedBy($"Um condutor com o Id '{condutorId}' não existe.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error GrupoVeiculoNullErro(Guid grupoVeiculoId)
    {
        return new Error("Grupo de Veículos inexistente")
            .CausedBy($"Um grupo de veículos com o id '{grupoVeiculoId}' não existe.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error VeiculoNullErro(Guid veiculoId)
    {
        return new Error("Veículo inexistente")
            .CausedBy($"Um veículo com o Id '{veiculoId}' não existe.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error PlanoCobrancaNullErro(Guid planoCobrancaId)
    {
        return new Error("Plano de Cobrança inexistente")
            .CausedBy($"Um plano de cobrança com o Id '{planoCobrancaId}' não existe.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error TaxaServicoNullErro(Guid taxaServicoId)
    {
        return new Error("Taxa ou Serviço inexistente")
            .CausedBy($"Uma taxa ou serviço com o Id '{taxaServicoId}' não existe.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error VeiculoNaoPertenceAoGrupoVeiculoErro(string veiculo, string grupoVeiculo)
    {
        return new Error("Veículo não pertence ao Grupo de Veículos")
            .CausedBy($"O veículo '{veiculo}' não pertence ao grupo de veículos '{grupoVeiculo}'.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error PlanoCobrancaNaoPertenceAoGrupoVeiculoErro(string planoCobranca, string grupoVeiculo)
    {
        return new Error("Plano de Cobrança não pertence ao Grupo de Veículos")
            .CausedBy($"O plano de cobrança '{planoCobranca}' não pertence ao grupo de veículos '{grupoVeiculo}'.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error AluguelFechadoErro(Guid idAluguel)
    {
        return new Error("Aluguel Concluído")
            .CausedBy($"O aluguel com Id'{idAluguel}' já foi concluído.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error AluguelAbertoErro(Guid idAluguel)
    {
        return new Error("Aluguel não Concluído")
            .CausedBy($"O aluguel com Id'{idAluguel}' não foi concluído.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error ValidadeCnhVencidaErro(string cpf)
    {
        return new Error("Validade vencida")
            .CausedBy($"O condutor de Cpf '{cpf}' esta com a Cnh vencida.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error VeiculoJaSelecionadoErro(string veiculo)
    {
        return new Error("Veículo pertence a outro aluguel")
            .CausedBy($"O veículo '{veiculo}' já pertence a um aluguel.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error KmRodadosErro()
    {
        return new Error("KM rodados")
            .CausedBy($"O KM antigo tem que ser menor que o KM atual.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }
    
    public static Error DataErradaErro()
    {
        return new Error("Data Errada")
            .CausedBy($"A data de retorno precisa ser posterior à data de entrada.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error PorcentagemTanqueObrigatoriaErro()
    {
        return new Error("Porcentagem Obrigatória")
            .CausedBy($"A porcentagem do tanque é obrigatória.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }
}
