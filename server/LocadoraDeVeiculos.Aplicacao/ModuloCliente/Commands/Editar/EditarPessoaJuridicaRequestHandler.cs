using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.Editar;

internal class EditarPessoaJuridicaRequestHandler(
    IRepositorioPessoaJuridica repositorioPessoaJuridica,
    IRepositorioCondutor repositorioCondutor,
    IContextoPersistencia contexto,
    IValidator<PessoaJuridica> validador
) : IRequestHandler<EditarPessoaJuridicaRequest, Result<EditarPessoaJuridicaResponse>>
{
    public async Task<Result<EditarPessoaJuridicaResponse>> Handle(EditarPessoaJuridicaRequest request, CancellationToken cancellationToken)
    {
        var pessoaJuridicaSelecionada = await repositorioPessoaJuridica.SelecionarPorIdAsync(request.Id);

        if (pessoaJuridicaSelecionada == null)
            return Result.Fail(ErrorResults.NotFoundError(request.Id));

        var condutorSelecionado = await repositorioCondutor.SelecionarPorIdAsync(request.CondutorId);

        if (condutorSelecionado == null)
            return Result.Fail(ClienteErrorResults.CondutorNullError(request.CondutorId));

        pessoaJuridicaSelecionada.Nome = request.Nome;
        pessoaJuridicaSelecionada.Telefone = request.Telefone;
        pessoaJuridicaSelecionada.Endereco = request.Endereco;
        pessoaJuridicaSelecionada.Cnpj = request.Cnpj;
        pessoaJuridicaSelecionada.Condutor = condutorSelecionado;

        var resultadoValidacao =
            await validador.ValidateAsync(pessoaJuridicaSelecionada, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ErrorResults.BadRequestError(erros));
        }

        var pessoasJuridicasRegistrados = await repositorioPessoaJuridica.SelecionarTodosAsync();

        if (CnpjDuplicado(pessoaJuridicaSelecionada, pessoasJuridicasRegistrados))
            return Result.Fail(ClienteErrorResults.CpfDuplicado(pessoaJuridicaSelecionada.Nome));

        try
        {
            await repositorioPessoaJuridica.EditarAsync(pessoaJuridicaSelecionada);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new EditarPessoaJuridicaResponse(pessoaJuridicaSelecionada.Id));
    }

    private bool CnpjDuplicado(PessoaJuridica pessoa, IList<PessoaJuridica> pessoasJuridicas)
    {
        return pessoasJuridicas
            .Where(x => x.Id != pessoa.Id)
            .Any(registro => string.Equals(
                registro.Cnpj,
                pessoa.Cnpj,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
