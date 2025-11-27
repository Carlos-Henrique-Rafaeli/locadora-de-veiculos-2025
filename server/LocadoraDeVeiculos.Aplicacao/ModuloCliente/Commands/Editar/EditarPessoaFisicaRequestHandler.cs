using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Editar;

internal class EditarPessoaFisicaRequestHandler(
    IRepositorioPessoaFisica repositorioPessoaFisica,
    IRepositorioPessoaJuridica repositorioPessoaJuridica,
    IContextoPersistencia contexto,
    IValidator<PessoaFisica> validador
) : IRequestHandler<EditarPessoaFisicaRequest, Result<EditarPessoaFisicaResponse>>
{
    public async Task<Result<EditarPessoaFisicaResponse>> Handle(EditarPessoaFisicaRequest request, CancellationToken cancellationToken)
    {
        var pessoaFisicaSelecionada = await repositorioPessoaFisica.SelecionarPorIdAsync(request.Id);

        if (pessoaFisicaSelecionada == null)
            return Result.Fail(ErrorResults.NotFoundError(request.Id));

        var pessoaJuridicaSelecionada = null as PessoaJuridica;

        if (request.PessoaJuridicaId.HasValue)
        {
            pessoaJuridicaSelecionada = await repositorioPessoaJuridica.SelecionarPorIdAsync(request.PessoaJuridicaId.Value);
            
            if (pessoaJuridicaSelecionada == null)
                return Result.Fail(ClienteErrorResults.PessoaJuridicaNullError(request.PessoaJuridicaId.Value));
        }

        pessoaFisicaSelecionada.Nome = request.Nome;
        pessoaFisicaSelecionada.Telefone = request.Telefone;
        pessoaFisicaSelecionada.Endereco = request.Endereco;
        pessoaFisicaSelecionada.Cpf = request.Cpf;
        pessoaFisicaSelecionada.Rg = request.Rg;
        pessoaFisicaSelecionada.Cnh = request.Cnh;
        pessoaFisicaSelecionada.PessoaJuridica = pessoaJuridicaSelecionada;

        var resultadoValidacao =
            await validador.ValidateAsync(pessoaFisicaSelecionada, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ErrorResults.BadRequestError(erros));
        }

        var pessoasFisicasRegistrados = await repositorioPessoaFisica.SelecionarTodosAsync();

        if (CpfDuplicado(pessoaFisicaSelecionada, pessoasFisicasRegistrados))
            return Result.Fail(ClienteErrorResults.CpfDuplicado(pessoaFisicaSelecionada.Nome));

        if (CnhDuplicada(pessoaFisicaSelecionada, pessoasFisicasRegistrados))
            return Result.Fail(ClienteErrorResults.CnhDuplicada(pessoaFisicaSelecionada.Nome));

        if (RgDuplicado(pessoaFisicaSelecionada, pessoasFisicasRegistrados))
            return Result.Fail(ClienteErrorResults.RgDuplicado(pessoaFisicaSelecionada.Nome));

        try
        {
            await repositorioPessoaFisica.EditarAsync(pessoaFisicaSelecionada);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new EditarPessoaFisicaResponse(pessoaFisicaSelecionada.Id));
    }

    private bool CpfDuplicado(PessoaFisica pessoa, IList<PessoaFisica> pessoasFisicas)
    {
        return pessoasFisicas
            .Where(x => x.Id != pessoa.Id)
            .Any(registro => string.Equals(
                registro.Cpf,
                pessoa.Cpf,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }

    private bool CnhDuplicada(PessoaFisica pessoa, IList<PessoaFisica> pessoasFisicas)
    {
        return pessoasFisicas
            .Where(x => x.Id != pessoa.Id)
            .Any(registro => string.Equals(
                registro.Cnh,
                pessoa.Cnh,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }

    private bool RgDuplicado(PessoaFisica pessoa, IList<PessoaFisica> pessoasFisicas)
    {
        return pessoasFisicas
            .Where(x => x.Id != pessoa.Id)
            .Any(registro => string.Equals(
                registro.Rg,
                pessoa.Rg,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
