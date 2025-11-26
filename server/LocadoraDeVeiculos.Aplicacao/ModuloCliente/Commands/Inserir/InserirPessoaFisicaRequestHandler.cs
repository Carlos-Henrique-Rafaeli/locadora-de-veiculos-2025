using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Inserir;

internal class InserirPessoaFisicaRequestHandler(
    IContextoPersistencia contexto,
    IRepositorioPessoaFisica repositorioPessoaFisica,
    IRepositorioPessoaJuridica repositorioPessoaJuridica,
    ITenantProvider tenantProvider,
    IValidator<PessoaFisica> validador
) : IRequestHandler<InserirPessoaFisicaRequest, Result<InserirPessoaFisicaResponse>>
{
    public async Task<Result<InserirPessoaFisicaResponse>> Handle(
        InserirPessoaFisicaRequest request, CancellationToken cancellationToken)
    {
        var pessoaJuridica = null as PessoaJuridica;

        if (request.PessoaJuridicaId.HasValue)
            pessoaJuridica = await repositorioPessoaJuridica.SelecionarPorIdAsync(request.PessoaJuridicaId.Value);

        var pessoaFisica = new PessoaFisica(
            request.Nome,
            request.Telefone,
            request.Endereco,
            request.Cpf,
            request.Rg,
            request.Cnh,
            pessoaJuridica
            )
        {
            UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault()
        };

        // validações
        var resultadoValidacao = await validador.ValidateAsync(pessoaFisica);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
               .Select(failure => failure.ErrorMessage)
               .ToList();

            return Result.Fail(ErrorResults.BadRequestError(erros));
        }

        var pessoasFisicasRegistrados = await repositorioPessoaFisica.SelecionarTodosAsync();

        if (CpfDuplicado(pessoaFisica, pessoasFisicasRegistrados))
            return Result.Fail(ClienteErrorResults.CpfDuplicado(pessoaFisica.Nome));

        if (CnhDuplicada(pessoaFisica, pessoasFisicasRegistrados))
            return Result.Fail(ClienteErrorResults.CnhDuplicada(pessoaFisica.Nome));

        if (RgDuplicado(pessoaFisica, pessoasFisicasRegistrados))
            return Result.Fail(ClienteErrorResults.RgDuplicado(pessoaFisica.Nome));

        // inserção
        try
        {
            await repositorioPessoaFisica.InserirAsync(pessoaFisica);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new InserirPessoaFisicaResponse(pessoaFisica.Id));
    }

    private bool CpfDuplicado(PessoaFisica pessoa, IList<PessoaFisica> pessoasFisicas)
    {
        return pessoasFisicas
            .Any(registro => string.Equals(
                registro.Cpf,
                pessoa.Cpf,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }

    private bool CnhDuplicada(PessoaFisica pessoa, IList<PessoaFisica> pessoasFisicas)
    {
        return pessoasFisicas
            .Any(registro => string.Equals(
                registro.Cnh,
                pessoa.Cnh,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }

    private bool RgDuplicado(PessoaFisica pessoa, IList<PessoaFisica> pessoasFisicas)
    {
        return pessoasFisicas
            .Any(registro => string.Equals(
                registro.Rg,
                pessoa.Rg,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
