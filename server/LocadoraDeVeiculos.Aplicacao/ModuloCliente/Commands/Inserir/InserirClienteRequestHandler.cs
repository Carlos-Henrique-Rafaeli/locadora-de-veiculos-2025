using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Inserir;

internal class InserirClienteRequestHandler(
    LocadoraDeVeiculosDbContext contexto,
    IRepositorioCliente repositorioCliente,
    ITenantProvider tenantProvider,
    IValidator<Cliente> validador
) : IRequestHandler<InserirClienteRequest, Result<InserirClienteResponse>>
{
    public async Task<Result<InserirClienteResponse>> Handle(
        InserirClienteRequest request, CancellationToken cancellationToken)
    {
        try
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
                EmpresaId = tenantProvider.EmpresaId.GetValueOrDefault()
            };

            var resultadoValidacao = await validador.ValidateAsync(cliente);

            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors
                   .Select(failure => failure.ErrorMessage)
                   .ToList();

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            var clientesRegistrados = await repositorioCliente.SelecionarTodosAsync();

            switch (cliente.TipoCliente)
            {
                case TipoCliente.PessoaFisica:
                    if (CpfDuplicado(cliente, clientesRegistrados))
                        return Result.Fail(ClienteResultadosErro.CpfDuplicadoErro(cliente.Cpf!));
                    break;

                case TipoCliente.PessoaJuridica:
                    if (CnpjDuplicado(cliente, clientesRegistrados))
                        return Result.Fail(ClienteResultadosErro.CnpjDuplicadoErro(cliente.Cnpj!));
                    break;

                default:
                    break;
            }


            await repositorioCliente.InserirAsync(cliente);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new InserirClienteResponse(cliente.Id));
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
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
