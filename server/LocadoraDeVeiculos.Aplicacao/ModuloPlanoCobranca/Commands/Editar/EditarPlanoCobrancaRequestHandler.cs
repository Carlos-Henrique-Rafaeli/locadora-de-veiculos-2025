using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Editar;

internal class EditarPlanoCobrancaRequestHandler(
    IRepositorioPlanoCobranca repositorioPlanoCobranca,
    IRepositorioGrupoVeiculos repositorioGrupoVeiculos,
    LocadoraDeVeiculosDbContext contexto,
    IValidator<PlanoCobranca> validador
) : IRequestHandler<EditarPlanoCobrancaRequest, Result<EditarPlanoCobrancaResponse>>
{
    public async Task<Result<EditarPlanoCobrancaResponse>> Handle(EditarPlanoCobrancaRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var planoCobrancaSelecionado = await repositorioPlanoCobranca.SelecionarPorIdAsync(request.Id);

            if (planoCobrancaSelecionado == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            var grupoVeiculoSelecionado = await repositorioGrupoVeiculos.SelecionarPorIdAsync(request.GrupoVeiculoId);

            if (grupoVeiculoSelecionado == null)
                return Result.Fail(PlanoCobrancaResultadosErro.GrupoVeiculoNullErro(request.GrupoVeiculoId));

            var resultadoValidacao =
                await validador.ValidateAsync(planoCobrancaSelecionado, cancellationToken);

            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors
                    .Select(failure => failure.ErrorMessage)
                    .ToList();

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            var planoCobrancaNovo = new PlanoCobranca(
                request.TipoPlano,
                grupoVeiculoSelecionado,
                request.ValorDiario,
                request.ValorKm,
                request.KmIncluso,
                request.ValorKmExcedente,
                request.ValorFixo
            );

            await repositorioPlanoCobranca.EditarAsync(request.Id, planoCobrancaNovo);

            await contexto.SaveChangesAsync(cancellationToken);
         
            return Result.Ok(new EditarPlanoCobrancaResponse(grupoVeiculoSelecionado.Id));
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
