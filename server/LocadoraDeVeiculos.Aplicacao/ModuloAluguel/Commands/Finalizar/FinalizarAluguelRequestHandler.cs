using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Finalizar;

internal class FinalizarAluguelRequestHandler(
    IRepositorioAluguel repositorioAluguel,
    IRepositorioConfiguracaoPreco repositorioConfiguracaoPreco,
    ITenantProvider tenantProvider,
    IContextoPersistencia contexto
) : IRequestHandler<FinalizarAluguelRequest, Result<FinalizarAluguelResponse>>
{
    public async Task<Result<FinalizarAluguelResponse>> Handle(FinalizarAluguelRequest request, CancellationToken cancellationToken)
    {
        var aluguelSelecionado = await repositorioAluguel.SelecionarPorIdAsync(request.Id);

        if (aluguelSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        if (!aluguelSelecionado.EstaAberto)
            return Result.Fail(AluguelErrorResults.AluguelFechadoError(aluguelSelecionado.Id));

        var kmRodados = request.kmAtual - request.kmInicial;

        if (kmRodados < 0)
            return Result.Fail(AluguelErrorResults.KmRodadosError());

        var atraso = request.DataRetorno > aluguelSelecionado.DataRetorno;

        if (aluguelSelecionado.DataEntrada > request.DataRetorno)
            return Result.Fail(AluguelErrorResults.DataErradaError());

        var precosSelecionados = await repositorioConfiguracaoPreco.SelecionarTodosAsync();
        var precos = precosSelecionados.FirstOrDefault();

        if (precos is null)
        {
            var config = new ConfiguracaoPreco
            {
                Gasolina = 6.99m,
                Diesel = 5.99m,
                Etanol = 4.99m,
                EmpresaId = tenantProvider.EmpresaId.GetValueOrDefault()
            };

            try
            {
                await repositorioConfiguracaoPreco.InserirAsync(config);

                await contexto.GravarAsync();
            }
            catch (Exception ex)
            {
                await contexto.RollbackAsync();

                return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
            }

            precos = await repositorioConfiguracaoPreco.SelecionarPorIdAsync(config.Id);
        }

        decimal precoCombustivel = 0;

        switch (aluguelSelecionado.Veiculo.TipoCombustivel)
        { 
            case TipoCombustivel.Gasolina:
                if (request.porcentagemTanque is null)
                    return Result.Fail(AluguelErrorResults.PorcentagemTanqueObrigatoriaError());

                precoCombustivel = aluguelSelecionado.Veiculo.CapacidadeTanque * request.porcentagemTanque.Value * precos!.Gasolina;
                break;
            
            case TipoCombustivel.Etanol:
                if (request.porcentagemTanque is null)
                    return Result.Fail(AluguelErrorResults.PorcentagemTanqueObrigatoriaError());

                precoCombustivel = aluguelSelecionado.Veiculo.CapacidadeTanque * request.porcentagemTanque.Value * precos!.Etanol;
                break;
            
            case TipoCombustivel.Diesel:
                if (request.porcentagemTanque is null)
                    return Result.Fail(AluguelErrorResults.PorcentagemTanqueObrigatoriaError());

                precoCombustivel = aluguelSelecionado.Veiculo.CapacidadeTanque * request.porcentagemTanque.Value * precos!.Diesel;
                break;
            
            default:
                break;
        }

        aluguelSelecionado.FinalizarAluguel(kmRodados, atraso, precoCombustivel);

        try
        {
            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new FinalizarAluguelResponse(aluguelSelecionado.Id, aluguelSelecionado.ValorFinal));
    }
}