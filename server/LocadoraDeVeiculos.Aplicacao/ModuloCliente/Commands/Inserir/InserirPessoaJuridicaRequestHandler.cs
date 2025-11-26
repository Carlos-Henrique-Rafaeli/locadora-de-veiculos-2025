using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Inserir;

internal class InserirPessoaJuridicaRequestHandler(
    IContextoPersistencia contexto,
    IRepositorioPessoaJuridica repositorioPessoaJuridica,
    IRepositorioCondutor repositoriocondutor,
    ITenantProvider tenantProvider,
    IValidator<PessoaJuridica> validador
) : IRequestHandler<InserirPessoaJuridicaRequest, Result<InserirPessoaJuridicaResponse>>
{
    public async Task<Result<InserirPessoaJuridicaResponse>> Handle(
        InserirPessoaJuridicaRequest request, CancellationToken cancellationToken)
    {
        var condutor = await repositoriocondutor.SelecionarPorIdAsync(request.CondutorId);

        if (condutor is null)
            return Result.Fail(ClienteErrorResults.CondutorNullError(request.CondutorId));

        var pessoaJuridica = new PessoaJuridica(
            request.Nome,
            request.Telefone,
            request.Endereco,
            request.Cnpj,
            condutor
            )
        {
            UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault()
        };

        // validações
        var resultadoValidacao = await validador.ValidateAsync(pessoaJuridica);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
               .Select(failure => failure.ErrorMessage)
               .ToList();

            return Result.Fail(ErrorResults.BadRequestError(erros));
        }

        var pessoasJuridicasRegistrados = await repositorioPessoaJuridica.SelecionarTodosAsync();

        if (CnpjDuplicado(pessoaJuridica, pessoasJuridicasRegistrados))
            return Result.Fail(ClienteErrorResults.CpfDuplicado(pessoaJuridica.Nome));
        
        // inserção
        try
        {
            await repositorioPessoaJuridica.InserirAsync(pessoaJuridica);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new InserirPessoaJuridicaResponse(pessoaJuridica.Id));
    }

    private bool CnpjDuplicado(PessoaJuridica pessoa, IList<PessoaJuridica> pessoasJuridicas)
    {
        return pessoasJuridicas
            .Any(registro => string.Equals(
                registro.Cnpj,
                pessoa.Cnpj,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
