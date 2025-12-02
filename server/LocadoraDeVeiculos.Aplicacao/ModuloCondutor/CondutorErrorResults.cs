using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor;

public abstract class CondutorErrorResults
{
    public static Error CpfDuplicado(string nome)
    {
        return new Error("CPF duplicado")
            .CausedBy($"Um condutor com o nome '{nome}' já foi cadastrado com este CPF.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error CnhDuplicada(string nome)
    {
        return new Error("CNH duplicada")
            .CausedBy($"Um condutor com o nome '{nome}' já foi cadastrado com esta CNH.")
            .WithMetadata("ErrorType", "BadRequest");
    }
    
    public static Error CnhVencida(string nome)
    {
        return new Error("CNH vencida")
            .CausedBy($"A CNH do condutor '{nome}' está vencida.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error ClienteNullError(Guid clienteId)
    {
        return new Error("Cliente inexistente")
            .CausedBy($"O cliente com id '{clienteId}' não existe.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error CondutorEmAluguelError(Guid Id)
    {
        return new Error("Condutor em aluguel")
            .CausedBy($"O condutor com id '{Id}' pertence a um aluguel.")
            .WithMetadata("ErrorType", "BadRequest");
    }
}
