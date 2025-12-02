using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente;

public abstract class ClienteErrorResults
{
    public static Error CpfDuplicado(string cpf)
    {
        return new Error("CPF duplicado")
            .CausedBy($"Um cliente com o Cpf '{cpf}' já foi cadastrado.")
            .WithMetadata("ErrorType", "BadRequest");
    }

    public static Error CnpjDuplicado(string cnpj)
    {
        return new Error("Cnpj duplicado")
            .CausedBy($"Um cliente com o cnpj '{cnpj}' já foi cadastrado.")
            .WithMetadata("ErrorType", "BadRequest");
    }
}
