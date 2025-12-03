using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Inserir;

public class InserirVeiculoRequestHandler(
    IContextoPersistencia contexto,
    IRepositorioVeiculo repositorioVeiculo,
    IRepositorioGrupoVeiculos repositorioGrupoVeiculos,
    ITenantProvider tenantProvider,
    IValidator<Veiculo> validador
) : IRequestHandler<InserirVeiculoRequest, Result<InserirVeiculoResponse>>
{
    public async Task<Result<InserirVeiculoResponse>> Handle(
        InserirVeiculoRequest request, CancellationToken cancellationToken)
    {
        var grupoVeiculo = await repositorioGrupoVeiculos.SelecionarPorIdAsync(request.GrupoVeiculoId);

        if (grupoVeiculo is null)
            return Result.Fail(VeiculoErrorResults.GrupoVeiculoNullError(request.GrupoVeiculoId));

        var veiculo = new Veiculo(
            grupoVeiculo, 
            request.Placa, 
            request.Modelo,
            request.Marca, 
            request.Cor, 
            request.TipoCombustivel,            
            request.CapacidadeTanque)
        {
            EmpresaId = tenantProvider.EmpresaId.GetValueOrDefault()
        };

        // validações
        var resultadoValidacao = await validador.ValidateAsync(veiculo);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
               .Select(failure => failure.ErrorMessage)
               .ToList();

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        var veiculosRegistrados = await repositorioVeiculo.SelecionarTodosAsync();

        if (PlacaDuplicada(veiculo, veiculosRegistrados))
            return Result.Fail(VeiculoErrorResults.PlacaDuplicadaError(veiculo.Placa));

        
        // inserção
        try
        {
            await repositorioVeiculo.InserirAsync(veiculo);
            grupoVeiculo.AdicionarVeiculo(veiculo);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new InserirVeiculoResponse(veiculo.Id));
    }

    private bool PlacaDuplicada(Veiculo veiculo, IList<Veiculo> veiculos)
    {
        return veiculos
            .Any(registro => string.Equals(
                registro.Placa,
                veiculo.Placa,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }
}
