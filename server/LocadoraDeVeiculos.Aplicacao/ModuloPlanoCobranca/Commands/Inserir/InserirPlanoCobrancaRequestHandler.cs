using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Inserir;

internal class InserirPlanoCobrancaRequestHandler(
    IContextoPersistencia contexto,
    IRepositorioPlanoCobranca repositorioPlanoCobranca,
    IRepositorioGrupoVeiculos repositorioGrupoVeiculos,
    ITenantProvider tenantProvider,
    IValidator<PlanoCobranca> validador
) : IRequestHandler<InserirPlanoCobrancaRequest, Result<InserirPlanoCobrancaResponse>>
{
    public async Task<Result<InserirPlanoCobrancaResponse>> Handle(
        InserirPlanoCobrancaRequest request, CancellationToken cancellationToken)
    {
        var grupoVeiculoSelecionado = await repositorioGrupoVeiculos.SelecionarPorIdAsync(request.GrupoVeiculoId);

        if (grupoVeiculoSelecionado is null)
            return Result.Fail(PlanoCobrancaErrorResults.GrupoVeiculoNullError(request.GrupoVeiculoId));

        var grupoVeiculo = new PlanoCobranca(
            request.TipoPlano,
            grupoVeiculoSelecionado,
            request.ValorDiario,
            request.ValorKm,
            request.KmIncluso,
            request.ValorKmExcedente,
            request.ValorFixo
            )
        {
            EmpresaId = tenantProvider.EmpresaId.GetValueOrDefault()
        };

        // validações
        var resultadoValidacao = await validador.ValidateAsync(grupoVeiculo);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
               .Select(failure => failure.ErrorMessage)
               .ToList();

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        var grupoVeiculosRegistrados = await repositorioPlanoCobranca.SelecionarTodosAsync();

        // inserção
        try
        {
            await repositorioPlanoCobranca.InserirAsync(grupoVeiculo);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new InserirPlanoCobrancaResponse(grupoVeiculo.Id));
    }
}
