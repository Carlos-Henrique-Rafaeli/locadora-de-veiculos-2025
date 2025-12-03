using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.Inserir;

internal class InserirTaxaServicoRequestHandler(
    IContextoPersistencia contexto,
    IRepositorioTaxaServico repositorioTaxaServico,
    ITenantProvider tenantProvider,
    IValidator<TaxaServico> validador
) : IRequestHandler<InserirTaxaServicoRequest, Result<InserirTaxaServicoResponse>>
{
    public async Task<Result<InserirTaxaServicoResponse>> Handle(
        InserirTaxaServicoRequest request, CancellationToken cancellationToken)
    {
        var taxaServico = new TaxaServico(request.Nome, request.Valor, request.TipoCobranca)
        {
            EmpresaId = tenantProvider.EmpresaId.GetValueOrDefault()
        };

        // validações
        var resultadoValidacao = await validador.ValidateAsync(taxaServico);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
               .Select(failure => failure.ErrorMessage)
               .ToList();

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        var taxasServicosRegistrados = await repositorioTaxaServico.SelecionarTodosAsync();

        if (NomeDuplicado(taxaServico, taxasServicosRegistrados))
            return Result.Fail(TaxaServicoErrorResults.NomeDuplicadoError(taxaServico.Nome));

        // inserção
        try
        {
            await repositorioTaxaServico.InserirAsync(taxaServico);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new InserirTaxaServicoResponse(taxaServico.Id));
    }

    private bool NomeDuplicado(TaxaServico taxaServico, IList<TaxaServico> taxasServicos)
    {
        return taxasServicos
            .Any(registro => string.Equals(
                registro.Nome,
                registro.Nome,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
