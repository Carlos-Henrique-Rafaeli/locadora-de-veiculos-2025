using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.Editar;

internal class EditarTaxaServicoRequestHandler(
    IRepositorioTaxaServico repositorioTaxaServico,
    IContextoPersistencia contexto,
    IValidator<TaxaServico> validador
) : IRequestHandler<EditarTaxaServicoRequest, Result<EditarTaxaServicoResponse>>
{
    public async Task<Result<EditarTaxaServicoResponse>> Handle(EditarTaxaServicoRequest request, CancellationToken cancellationToken)
    {
        var taxaServicoSelecionada = await repositorioTaxaServico.SelecionarPorIdAsync(request.Id);

        if (taxaServicoSelecionada == null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        taxaServicoSelecionada.Nome = request.Nome;
        taxaServicoSelecionada.Valor = request.Valor;
        taxaServicoSelecionada.TipoCobranca = request.TipoCobranca;

        var resultadoValidacao =
            await validador.ValidateAsync(taxaServicoSelecionada, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        var taxasServicosSelecionados = await repositorioTaxaServico.SelecionarTodosAsync();

        if (NomeDuplicado(taxaServicoSelecionada, taxasServicosSelecionados))
            return Result.Fail(TaxaServicoErrorResults.NomeDuplicadoError(taxaServicoSelecionada.Nome));

        try
        {
            await repositorioTaxaServico.EditarAsync(taxaServicoSelecionada);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new EditarTaxaServicoResponse(taxaServicoSelecionada.Id));
    }

    private bool NomeDuplicado(TaxaServico taxaServico, IList<TaxaServico> taxasServicos)
    {
        return taxasServicos
            .Where(r => r.Id != taxaServico.Id)
            .Any(registro => string.Equals(
                registro.Nome,
                taxaServico.Nome,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
