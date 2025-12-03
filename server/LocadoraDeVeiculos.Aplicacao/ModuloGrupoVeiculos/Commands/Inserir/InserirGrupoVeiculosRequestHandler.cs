using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Inserir;

internal class InserirGrupoVeiculosRequestHandler(
    IContextoPersistencia contexto,
    IRepositorioGrupoVeiculos repositorioGrupoVeiculos,
    ITenantProvider tenantProvider,
    IValidator<GrupoVeiculo> validador
) : IRequestHandler<InserirGrupoVeiculosRequest, Result<InserirGrupoVeiculosResponse>>
{
    public async Task<Result<InserirGrupoVeiculosResponse>> Handle(
        InserirGrupoVeiculosRequest request, CancellationToken cancellationToken)
    {
        var grupoVeiculo = new GrupoVeiculo(request.nome)
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

        var grupoVeiculosRegistrados = await repositorioGrupoVeiculos.SelecionarTodosAsync();

        if (NomeDuplicado(grupoVeiculo, grupoVeiculosRegistrados))
            return Result.Fail(GrupoVeiculosErrorResults.NomeDuplicadoError(grupoVeiculo.Nome));


        // inserção
        try
        {
            await repositorioGrupoVeiculos.InserirAsync(grupoVeiculo);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new InserirGrupoVeiculosResponse(grupoVeiculo.Id));
    }

    private bool NomeDuplicado(GrupoVeiculo grupoVeiculo, IList<GrupoVeiculo> grupoVeiculos)
    {
        return grupoVeiculos
            .Any(registro => string.Equals(
                registro.Nome,
                grupoVeiculo.Nome,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
