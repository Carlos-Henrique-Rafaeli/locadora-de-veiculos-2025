using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Editar;

public class EditarGrupoVeiculosRequestHandler(
    IRepositorioGrupoVeiculos repositorioGrupoVeiculo,
    LocadoraDeVeiculosDbContext contexto,
    IValidator<GrupoVeiculo> validador
) : IRequestHandler<EditarGrupoVeiculosRequest, Result<EditarGrupoVeiculosResponse>>
{
    public async Task<Result<EditarGrupoVeiculosResponse>> Handle(EditarGrupoVeiculosRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var grupoVeiculoSelecionado = await repositorioGrupoVeiculo.SelecionarPorIdAsync(request.Id);

            if (grupoVeiculoSelecionado == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            var resultadoValidacao =
                await validador.ValidateAsync(grupoVeiculoSelecionado, cancellationToken);

            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors
                    .Select(failure => failure.ErrorMessage)
                    .ToList();

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            var grupoVeiculos = await repositorioGrupoVeiculo.SelecionarTodosAsync();

            if (NomeDuplicado(grupoVeiculoSelecionado, grupoVeiculos))
                return Result.Fail(GrupoVeiculosResultadosErro.NomeDuplicadoErro(grupoVeiculoSelecionado.Nome));

            var grupoVeiculoNovo = new GrupoVeiculo(request.Nome);

            await repositorioGrupoVeiculo.EditarAsync(request.Id, grupoVeiculoNovo);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new EditarGrupoVeiculosResponse(grupoVeiculoSelecionado.Id));
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
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
