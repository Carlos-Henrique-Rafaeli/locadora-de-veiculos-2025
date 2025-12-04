using FluentResults;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente;

public abstract class ClienteResultadosErro
{
    public static Error CpfDuplicadoErro(string cpf)
    {
        return new Error("CPF duplicado")
            .CausedBy($"Um cliente com o Cpf '{cpf}' já foi cadastrado.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error CnpjDuplicadoErro(string cnpj)
    {
        return new Error("Cnpj duplicado")
            .CausedBy($"Um cliente com o cnpj '{cnpj}' já foi cadastrado.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }

    public static Error ClienteEmCondutorErro(Guid id)
    {
        return new Error("Cliente em condutor")
            .CausedBy($"O cliente com Id '{id}' pertence a um condutor.")
            .WithMetadata("ErrorType", "RequisicaoInvalida");
    }
}
