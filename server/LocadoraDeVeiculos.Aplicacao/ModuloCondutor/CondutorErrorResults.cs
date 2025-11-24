using FluentResults;

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
}
