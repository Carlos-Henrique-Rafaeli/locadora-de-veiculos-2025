using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.Editar;

internal class EditarTaxaServicoRequestHandler(
    IRepositorioTaxaServico repositorioTaxaServico,
    LocadoraDeVeiculosDbContext contexto,
    IValidator<TaxaServico> validador
) : IRequestHandler<EditarTaxaServicoRequest, Result<EditarTaxaServicoResponse>>
{
    public async Task<Result<EditarTaxaServicoResponse>> Handle(EditarTaxaServicoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var taxaServicoSelecionada = await repositorioTaxaServico.SelecionarPorIdAsync(request.Id);

            if (taxaServicoSelecionada == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            var taxaServicoNovo = new TaxaServico(
                request.Nome,
                request.Valor,
                request.TipoCobranca
            );

            var resultadoValidacao =
                await validador.ValidateAsync(taxaServicoNovo, cancellationToken);

            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors
                    .Select(failure => failure.ErrorMessage)
                    .ToList();

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            var taxasServicosSelecionados = await repositorioTaxaServico.SelecionarTodosAsync();

            if (NomeDuplicado(taxaServicoNovo, taxasServicosSelecionados, request.Id))
                return Result.Fail(TaxaServicoResultadosErro.NomeDuplicadoErro(taxaServicoNovo.Nome));

            await repositorioTaxaServico.EditarAsync(request.Id, taxaServicoNovo);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new EditarTaxaServicoResponse(taxaServicoSelecionada.Id));
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    private bool NomeDuplicado(TaxaServico taxaServico, IList<TaxaServico> taxasServicos, Guid taxaServicoAntigo)
    {
        return taxasServicos
            .Where(r => r.Id != taxaServicoAntigo)
            .Any(registro => string.Equals(
                registro.Nome,
                taxaServico.Nome,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
