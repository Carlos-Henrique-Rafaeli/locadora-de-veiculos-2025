using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands.Editar;

internal class EditarPlanoCobrancaRequestHandler(
    IRepositorioPlanoCobranca repositorioPlanoCobranca,
    IRepositorioGrupoVeiculos repositorioGrupoVeiculos,
    IContextoPersistencia contexto,
    IValidator<PlanoCobranca> validador
) : IRequestHandler<EditarPlanoCobrancaRequest, Result<EditarPlanoCobrancaResponse>>
{
    public async Task<Result<EditarPlanoCobrancaResponse>> Handle(EditarPlanoCobrancaRequest request, CancellationToken cancellationToken)
    {
        var grupoVeiculoSelecionado = await repositorioPlanoCobranca.SelecionarPorIdAsync(request.Id);

        if (grupoVeiculoSelecionado == null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        var grupoSelecionado = await repositorioGrupoVeiculos.SelecionarPorIdAsync(request.GrupoVeiculoId);

        if (grupoSelecionado == null)
            return Result.Fail(PlanoCobrancaErrorResults.GrupoVeiculoNullError(request.GrupoVeiculoId));

        grupoVeiculoSelecionado.TipoPlano = request.TipoPlano;
        grupoVeiculoSelecionado.GrupoVeiculo = grupoSelecionado;
        grupoVeiculoSelecionado.ValorDiario = request.ValorDiario;
        grupoVeiculoSelecionado.ValorKm = request.ValorKm;
        grupoVeiculoSelecionado.KmIncluso = request.KmIncluso;
        grupoVeiculoSelecionado.ValorKmExcedente = request.ValorKmExcedente;
        grupoVeiculoSelecionado.ValorFixo = request.ValorFixo;

        var resultadoValidacao =
            await validador.ValidateAsync(grupoVeiculoSelecionado, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        var grupoVeiculos = await repositorioPlanoCobranca.SelecionarTodosAsync();

        try
        {
            await repositorioPlanoCobranca.EditarAsync(grupoVeiculoSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new EditarPlanoCobrancaResponse(grupoVeiculoSelecionado.Id));
    }
}
