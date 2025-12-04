using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.Inserir;

internal class InserirTaxaServicoRequestHandler(
    LocadoraDeVeiculosDbContext contexto,
    IRepositorioTaxaServico repositorioTaxaServico,
    ITenantProvider tenantProvider,
    IValidator<TaxaServico> validador
) : IRequestHandler<InserirTaxaServicoRequest, Result<InserirTaxaServicoResponse>>
{
    public async Task<Result<InserirTaxaServicoResponse>> Handle(
        InserirTaxaServicoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var taxaServico = new TaxaServico(request.Nome, request.Valor, request.TipoCobranca)
            {
                EmpresaId = tenantProvider.EmpresaId.GetValueOrDefault()
            };

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
                return Result.Fail(TaxaServicoResultadosErro.NomeDuplicadoErro(taxaServico.Nome));

            await repositorioTaxaServico.InserirAsync(taxaServico);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new InserirTaxaServicoResponse(taxaServico.Id));
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
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
