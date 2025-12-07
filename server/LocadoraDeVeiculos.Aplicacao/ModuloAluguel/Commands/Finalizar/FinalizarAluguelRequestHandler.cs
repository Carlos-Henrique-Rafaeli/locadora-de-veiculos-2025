using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Finalizar;

internal class FinalizarAluguelRequestHandler(
    IRepositorioAluguel repositorioAluguel,
    IRepositorioConfiguracaoPreco repositorioConfiguracaoPreco,
    ITenantProvider tenantProvider,
    LocadoraDeVeiculosDbContext contexto,
    IValidator<FinalizarAluguelRequest> validador
) : IRequestHandler<FinalizarAluguelRequest, Result<FinalizarAluguelResponse>>
{
    public async Task<Result<FinalizarAluguelResponse>> Handle(FinalizarAluguelRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var aluguelSelecionado = await repositorioAluguel.SelecionarPorIdAsync(request.Id);

            if (aluguelSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            if (!aluguelSelecionado.EstaAberto)
                return Result.Fail(AluguelResultadosErro.AluguelFechadoErro(aluguelSelecionado.Id));

            var resultadoValidacao = await validador.ValidateAsync(request);

            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors
                   .Select(failure => failure.ErrorMessage)
                   .ToList();

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

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

                    await contexto.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
                }

                precos = await repositorioConfiguracaoPreco.SelecionarPorIdAsync(config.Id);
            }

            decimal precoCombustivel = 0;

            if (!request.tanqueCheio)
            {
                switch (aluguelSelecionado.Veiculo.TipoCombustivel)
                {
                    case TipoCombustivel.Gasolina:
                        precoCombustivel = aluguelSelecionado.Veiculo.CapacidadeTanque * request.porcentagemTanque.Value * precos!.Gasolina;
                        break;

                    case TipoCombustivel.Etanol:
                        precoCombustivel = aluguelSelecionado.Veiculo.CapacidadeTanque * request.porcentagemTanque.Value * precos!.Etanol;
                        break;

                    case TipoCombustivel.Diesel:
                        precoCombustivel = aluguelSelecionado.Veiculo.CapacidadeTanque * request.porcentagemTanque.Value * precos!.Diesel;
                        break;

                    default:
                        break;
                }
            }

            var kmRodados = request.kmAtual - request.kmInicial;

            if (request.DataRetorno < aluguelSelecionado.DataEntrada)
                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("A data de retorno deve ser maior ou igual a data de entrada"));

            aluguelSelecionado.FinalizarAluguel(kmRodados, precoCombustivel, request.DataRetorno);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new FinalizarAluguelResponse(aluguelSelecionado.Id, aluguelSelecionado.ValorFinal));

        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

    }
}