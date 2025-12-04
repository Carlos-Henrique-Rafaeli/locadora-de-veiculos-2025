using FluentResults;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor;

public abstract class CondutorResultadosErro
{
    public static Error CpfDuplicadoErro(string nome)
    {
        return new Error("CPF duplicado")
            .CausedBy($"Um condutor com o nome '{nome}' já foi cadastrado com este CPF.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error CnhDuplicadaErro(string nome)
    {
        return new Error("CNH duplicada")
            .CausedBy($"Um condutor com o nome '{nome}' já foi cadastrado com esta CNH.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }
    
    public static Error CnhVencidaErro(string nome)
    {
        return new Error("CNH vencida")
            .CausedBy($"A CNH do condutor '{nome}' está vencida.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error ClienteNullErro(Guid clienteId)
    {
        return new Error("Cliente inexistente")
            .CausedBy($"O cliente com id '{clienteId}' não existe.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error AluguelAtivoErro()
    {
        return new Error("Aluguel ativo")
            .CausedBy($"O condutor pertence a um aluguel.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }
}
