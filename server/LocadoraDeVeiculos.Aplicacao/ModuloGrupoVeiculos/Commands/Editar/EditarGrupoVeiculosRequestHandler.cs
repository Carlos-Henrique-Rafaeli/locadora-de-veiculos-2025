using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Editar;

public class EditarGrupoVeiculosRequestHandler(
    IRepositorioGrupoVeiculos repositorioGrupoVeiculo,
    IContextoPersistencia contexto,
    IValidator<GrupoVeiculo> validador
) : IRequestHandler<EditarGrupoVeiculosRequest, Result<EditarGrupoVeiculosResponse>>
{
    public async Task<Result<EditarGrupoVeiculosResponse>> Handle(EditarGrupoVeiculosRequest request, CancellationToken cancellationToken)
    {
        var grupoVeiculoSelecionado = await repositorioGrupoVeiculo.SelecionarPorIdAsync(request.Id);

        if (grupoVeiculoSelecionado == null)
            return Result.Fail(ErrorResults.NotFoundError(request.Id));

        grupoVeiculoSelecionado.Nome = request.Nome;

        var resultadoValidacao =
            await validador.ValidateAsync(grupoVeiculoSelecionado, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ErrorResults.BadRequestError(erros));
        }

        var grupoVeiculos = await repositorioGrupoVeiculo.SelecionarTodosAsync();

        if (NomeDuplicado(grupoVeiculoSelecionado, grupoVeiculos))
            return Result.Fail(GrupoVeiculosErrorResults.NomeDuplicadoError(grupoVeiculoSelecionado.Nome));


        try
        {
            await repositorioGrupoVeiculo.EditarAsync(grupoVeiculoSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new EditarGrupoVeiculosResponse(grupoVeiculoSelecionado.Id));
    }

    private bool NomeDuplicado(GrupoVeiculo grupoVeiculo, IList<GrupoVeiculo> grupoVeiculos)
    {
        return grupoVeiculos
            .Where(r => r.Id != grupoVeiculo.Id)
            .Any(registro => string.Equals(
                registro.Nome,
                grupoVeiculo.Nome,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
