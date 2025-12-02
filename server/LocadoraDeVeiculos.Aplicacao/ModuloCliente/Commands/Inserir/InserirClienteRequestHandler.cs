using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Inserir;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Inserir;

internal class InserirClienteRequestHandler(
    IContextoPersistencia contexto,
    IRepositorioCliente repositorioCliente,
    ITenantProvider tenantProvider,
    IValidator<Cliente> validador
) : IRequestHandler<InserirClienteRequest, Result<InserirClienteResponse>>
{
    public async Task<Result<InserirClienteResponse>> Handle(
        InserirClienteRequest request, CancellationToken cancellationToken)
    {
        var cliente = new Cliente(
            request.TipoCliente,
            request.Nome,
            request.Telefone,
            request.Cpf,
            request.Cnpj,
            request.Estado,
            request.Cidade,
            request.Bairro,
            request.Rua,
            request.Numero
            )
        {
            UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault()
        };

        // validações
        var resultadoValidacao = await validador.ValidateAsync(cliente);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
               .Select(failure => failure.ErrorMessage)
               .ToList();

            return Result.Fail(ErrorResults.BadRequestError(erros));
        }

        var clientesRegistrados = await repositorioCliente.SelecionarTodosAsync();

        switch (cliente.TipoCliente)
        {
            case TipoCliente.PessoaFisica:
                if (CpfDuplicado(cliente, clientesRegistrados))
                    return Result.Fail(ClienteErrorResults.CpfDuplicado(cliente.Cpf!));
                break;
            
            case TipoCliente.PessoaJuridica:
                if (CnpjDuplicado(cliente, clientesRegistrados))
                    return Result.Fail(ClienteErrorResults.CnpjDuplicado(cliente.Cnpj!));
                break;
            
            default:
                break;
        }

        // inserção
        try
        {
            await repositorioCliente.InserirAsync(cliente);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new InserirClienteResponse(cliente.Id));
    }

    private bool CpfDuplicado(Cliente cliente, IList<Cliente> clientes)
    {
        return clientes
            .Any(registro => string.Equals(
                registro.Cpf,
                cliente.Cpf,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }

    private bool CnpjDuplicado(Cliente cliente, IList<Cliente> clientes)
    {
        return clientes
            .Any(registro => string.Equals(
                registro.Cnpj,
                cliente.Cnpj,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
