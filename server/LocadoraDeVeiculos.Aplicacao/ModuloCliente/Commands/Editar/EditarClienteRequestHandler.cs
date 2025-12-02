using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Editar;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Editar;

internal class EditarClienteRequestHandler(
    IRepositorioCliente repositorioCliente,
    IContextoPersistencia contexto,
    IValidator<Cliente> validador
) : IRequestHandler<EditarClienteRequest, Result<EditarClienteResponse>>
{
    public async Task<Result<EditarClienteResponse>> Handle(EditarClienteRequest request, CancellationToken cancellationToken)
    {
        var clienteSelecionado = await repositorioCliente.SelecionarPorIdAsync(request.Id);

        if (clienteSelecionado == null)
            return Result.Fail(ErrorResults.NotFoundError(request.Id));

        clienteSelecionado.TipoCliente = request.TipoCliente;
        clienteSelecionado.Nome = request.Nome;
        clienteSelecionado.Telefone = request.Telefone;
        clienteSelecionado.Cpf = request.Cpf;
        clienteSelecionado.Cnpj = request.Cnpj;
        clienteSelecionado.Estado = request.Estado;
        clienteSelecionado.Cidade = request.Cidade;
        clienteSelecionado.Bairro = request.Bairro;
        clienteSelecionado.Rua = request.Rua;
        clienteSelecionado.Numero = request.Numero;

        var resultadoValidacao =
            await validador.ValidateAsync(clienteSelecionado, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ErrorResults.BadRequestError(erros));
        }

        var clientesRegistrados = await repositorioCliente.SelecionarTodosAsync();

        switch (clienteSelecionado.TipoCliente)
        {
            case TipoCliente.PessoaFisica:
                if (CpfDuplicado(clienteSelecionado, clientesRegistrados))
                    return Result.Fail(ClienteErrorResults.CpfDuplicado(clienteSelecionado.Cpf));
                
                break;

            case TipoCliente.PessoaJuridica:
                if (CnpjDuplicado(clienteSelecionado, clientesRegistrados))
                    return Result.Fail(ClienteErrorResults.CnpjDuplicado(clienteSelecionado.Cnpj));
                
                break;

            default:
                break;
        }

        try
        {
            await repositorioCliente.EditarAsync(clienteSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new EditarClienteResponse(clienteSelecionado.Id));
    }

    private bool CpfDuplicado(Cliente cliente, IList<Cliente> clientes)
    {
        return clientes
            .Where(r => r.Id != cliente.Id)
            .Any(registro => string.Equals(
                registro.Cpf,
                cliente.Cpf,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }

    private bool CnpjDuplicado(Cliente cliente, IList<Cliente> clientes)
    {
        return clientes
            .Where(r => r.Id != cliente.Id)
            .Any(registro => string.Equals(
                registro.Cnpj,
                cliente.Cnpj,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
