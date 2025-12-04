using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Editar;

public class EditarVeiculoRequestHandler(
    IRepositorioVeiculo repositorioVeiculo,
    IRepositorioGrupoVeiculos repositorioGrupoVeiculo,
    LocadoraDeVeiculosDbContext contexto,
    IValidator<Veiculo> validador
) : IRequestHandler<EditarVeiculoRequest, Result<EditarVeiculoResponse>>
{
    public async Task<Result<EditarVeiculoResponse>> Handle(EditarVeiculoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var veiculoSelecionado = await repositorioVeiculo.SelecionarPorIdAsync(request.Id);

            if (veiculoSelecionado == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            veiculoSelecionado.GrupoVeiculo.RemoverVeiculo(veiculoSelecionado);

            var grupoVeiculoSelecionado = await repositorioGrupoVeiculo.SelecionarPorIdAsync(veiculoSelecionado.GrupoVeiculo.Id);

            if (grupoVeiculoSelecionado is null)
                return Result.Fail(VeiculoResultadosErro.GrupoVeiculoNullErro(request.GrupoVeiculoId));

            
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
                return Result.Fail(VeiculoResultadosErro.PlacaDuplicadaErro(veiculoSelecionado.Placa));

            var veiculoNovo = new Veiculo(
                grupoVeiculoSelecionado,
                request.Placa,
                request.Marca,
                request.Modelo,
                request.Cor,
                request.TipoCombustivel,
                request.CapacidadeTanque
            );

            await repositorioVeiculo.EditarAsync(request.Id, veiculoNovo);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new EditarVeiculoResponse(grupoVeiculoSelecionado.Id));
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
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
