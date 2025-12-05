using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Editar;

internal class EditarClienteRequestHandler(
    IRepositorioCliente repositorioCliente,
    LocadoraDeVeiculosDbContext contexto,
    IValidator<Cliente> validador
) : IRequestHandler<EditarClienteRequest, Result<EditarClienteResponse>>
{
    public async Task<Result<EditarClienteResponse>> Handle(EditarClienteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var clienteSelecionado = await repositorioCliente.SelecionarPorIdAsync(request.Id);

            if (clienteSelecionado == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            var clienteNovo = new Cliente(
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
                );

            var resultadoValidacao =
                await validador.ValidateAsync(clienteNovo, cancellationToken);

            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors
                    .Select(failure => failure.ErrorMessage)
                    .ToList();

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            var clientesRegistrados = await repositorioCliente.SelecionarTodosAsync();

            switch (clienteNovo.TipoCliente)
            {
                case TipoCliente.PessoaFisica:
                    if (CpfDuplicado(clienteNovo, clientesRegistrados, request.Id))
                        return Result.Fail(ClienteResultadosErro.CpfDuplicadoErro(clienteNovo.Cpf));

                    break;

                case TipoCliente.PessoaJuridica:
                    if (CnpjDuplicado(clienteNovo, clientesRegistrados, request.Id))
                        return Result.Fail(ClienteResultadosErro.CnpjDuplicadoErro(clienteNovo.Cnpj));

                    break;

                default:
                    break;
            }

            await repositorioCliente.EditarAsync(request.Id, clienteNovo);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new EditarClienteResponse(clienteSelecionado.Id));
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    private bool CpfDuplicado(Cliente cliente, IList<Cliente> clientes, Guid clienteAntigo)
    {
        return clientes
            .Where(r => r.Id != clienteAntigo)
            .Any(registro => string.Equals(
                registro.Cpf,
                cliente.Cpf,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }

    private bool CnpjDuplicado(Cliente cliente, IList<Cliente> clientes, Guid clienteAntigo)
    {
        return clientes
            .Where(r => r.Id != clienteAntigo)
            .Any(registro => string.Equals(
                registro.Cnpj,
                cliente.Cnpj,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
