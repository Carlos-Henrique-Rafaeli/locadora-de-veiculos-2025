using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Editar;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Editar;

public class EditarVeiculoRequestHandler(
    IRepositorioVeiculo repositorioVeiculo,
    IRepositorioGrupoVeiculos repositorioGrupoVeiculo,
    IContextoPersistencia contexto,
    IValidator<Veiculo> validador
) : IRequestHandler<EditarVeiculoRequest, Result<EditarVeiculoResponse>>
{
    public async Task<Result<EditarVeiculoResponse>> Handle(EditarVeiculoRequest request, CancellationToken cancellationToken)
    {
        var veiculoSelecionado = await repositorioVeiculo.SelecionarPorIdAsync(request.Id);

        if (veiculoSelecionado == null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        veiculoSelecionado.GrupoVeiculo.RemoverVeiculo(veiculoSelecionado);

        var grupoVeiculoSelecionado = await repositorioGrupoVeiculo.SelecionarPorIdAsync(veiculoSelecionado.GrupoVeiculo.Id);

        if (grupoVeiculoSelecionado is null)
            return Result.Fail(VeiculoErrorResults.GrupoVeiculoNullError(request.GrupoVeiculoId));

        veiculoSelecionado.Placa = request.Placa;
        veiculoSelecionado.Marca = request.Marca;
        veiculoSelecionado.Modelo = request.Modelo;
        veiculoSelecionado.Cor = request.Cor;
        veiculoSelecionado.TipoCombustivel = request.TipoCombustivel;
        veiculoSelecionado.CapacidadeTanque = request.CapacidadeTanque;
        veiculoSelecionado.GrupoVeiculo.AdicionarVeiculo(veiculoSelecionado);

        var resultadoValidacao =
            await validador.ValidateAsync(veiculoSelecionado, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        var grupoVeiculos = await repositorioVeiculo.SelecionarTodosAsync();

        if (PlacaDuplicada(veiculoSelecionado, grupoVeiculos))
            return Result.Fail(VeiculoErrorResults.PlacaDuplicadaError(veiculoSelecionado.Placa));

        try
        {
            await repositorioGrupoVeiculo.EditarAsync(grupoVeiculoSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new EditarVeiculoResponse(grupoVeiculoSelecionado.Id));
    }

    private bool PlacaDuplicada(Veiculo veiculo, IList<Veiculo> veiculos)
    {
        return veiculos
            .Where(r => r.Id != veiculo.Id)
            .Any(registro => string.Equals(
                registro.Placa,
                veiculo.Placa,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
