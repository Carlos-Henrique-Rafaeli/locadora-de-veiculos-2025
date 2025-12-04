using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Inserir;

internal class InserirGrupoVeiculosRequestHandler(
    LocadoraDeVeiculosDbContext contexto,
    IRepositorioGrupoVeiculos repositorioGrupoVeiculos,
    ITenantProvider tenantProvider,
    IValidator<GrupoVeiculo> validador
) : IRequestHandler<InserirGrupoVeiculosRequest, Result<InserirGrupoVeiculosResponse>>
{
    public async Task<Result<InserirGrupoVeiculosResponse>> Handle(
        InserirGrupoVeiculosRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var grupoVeiculo = new GrupoVeiculo(request.nome)
            {
                EmpresaId = tenantProvider.EmpresaId.GetValueOrDefault()
            };

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
                return Result.Fail(GrupoVeiculosResultadosErro.NomeDuplicadoErro(grupoVeiculo.Nome));

            await repositorioGrupoVeiculos.InserirAsync(grupoVeiculo);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new InserirGrupoVeiculosResponse(grupoVeiculo.Id));
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
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
