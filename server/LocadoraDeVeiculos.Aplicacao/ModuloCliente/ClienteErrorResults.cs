using FluentResults;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente;

public abstract class ClienteErrorResults
{
    public static Error CpfDuplicado(string nome)
    {
        return new Error("CPF duplicado")
            .CausedBy($"Um cliente com o nome '{nome}' já foi cadastrado com este CPF.")
            .WithMetadata("ErrorType", "BadRequest");
    }
    public static Error CnhDuplicada(string nome)
    {
        return new Error("CNH duplicada")
            .CausedBy($"Um cliente com o nome '{nome}' já foi cadastrado com esta CNH.")
            .WithMetadata("ErrorType", "BadRequest");
    }
    public static Error RgDuplicado(string nome)
    {
        return new Error("RG duplicado")
            .CausedBy($"Um cliente com o nome '{nome}' já foi cadastrado com este RG.")
            .WithMetadata("ErrorType", "BadRequest");
    }
    public static Error CondutorNullError(Guid condutorId)
    {
        return new Error("Condutor inexistente")
            .CausedBy($"Um condutor com o Id '{condutorId}' não existe.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error PessoaJuridicaNullError(Guid pessoaJuridicaId)
    {
        return new Error("Pessoa Jurídica inexistente")
            .CausedBy($"Uma pessoa jurídica com o Id '{pessoaJuridicaId}' não existe.")
            .WithMetadata("ErrorType", "BadRequest");
    }
}
