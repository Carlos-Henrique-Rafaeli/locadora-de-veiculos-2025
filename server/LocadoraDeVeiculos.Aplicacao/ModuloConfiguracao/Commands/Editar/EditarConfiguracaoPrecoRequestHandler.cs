using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloConfiguracao.Commands.Editar;

internal class EditarConfiguracaoPrecoRequestHandler(
    IRepositorioConfiguracaoPreco repositorioConfiguracaoPreco,
    LocadoraDeVeiculosDbContext contexto,
    ITenantProvider tenantProvider,
    IValidator<ConfiguracaoPreco> validador
) : IRequestHandler<EditarConfiguracaoPrecoRequest, Result<EditarConfiguracaoPrecoResponse>>
{
    public async Task<Result<EditarConfiguracaoPrecoResponse>> Handle(EditarConfiguracaoPrecoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var configuracoesPrecosSelecionadas = await repositorioConfiguracaoPreco.SelecionarTodosAsync();

            var configuracaoPrecoSelecionada = configuracoesPrecosSelecionadas.FirstOrDefault();

            if (configuracaoPrecoSelecionada == null)
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

                configuracaoPrecoSelecionada = await repositorioConfiguracaoPreco.SelecionarPorIdAsync(config.Id);
            }

            var configNova = new ConfiguracaoPreco(
                request.Gasolina,
                request.Etanol,
                request.Diesel
            );

            var resultadoValidacao =
                await validador.ValidateAsync(configNova, cancellationToken);

            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors
                    .Select(failure => failure.ErrorMessage)
                    .ToList();

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            await repositorioConfiguracaoPreco.EditarAsync(configuracaoPrecoSelecionada.Id, configNova);

            await contexto.SaveChangesAsync(cancellationToken);
    
            return Result.Ok(new EditarConfiguracaoPrecoResponse());
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
