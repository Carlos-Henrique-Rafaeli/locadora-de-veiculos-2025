using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using MediatR;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Inserir;

internal class InserirAluguelRequestHandler(
    LocadoraDeVeiculosDbContext contexto,
    IRepositorioAluguel repositorioAluguel,
    IRepositorioCondutor repositorioCondutor,
    IRepositorioGrupoVeiculos repositorioGrupoVeiculo,
    IRepositorioVeiculo repositorioVeiculo,
    IRepositorioPlanoCobranca repositorioPlanoCobrnca,
    IRepositorioTaxaServico repositorioTaxaServico,
    ITenantProvider tenantProvider,
    IValidator<Aluguel> validador
) : IRequestHandler<InserirAluguelRequest, Result<InserirAluguelResponse>>
{
    public async Task<Result<InserirAluguelResponse>> Handle(
        InserirAluguelRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var condutorSelecionado = await repositorioCondutor.SelecionarPorIdAsync(request.CondutorId);

            if (condutorSelecionado is null)
                return Result.Fail(AluguelResultadosErro.CondutorNullErro(request.CondutorId));

            if (condutorSelecionado.ValidadeCnh < DateTime.Today)
                return Result.Fail(AluguelResultadosErro.ValidadeCnhVencidaErro(condutorSelecionado.Cpf));

            var grupoVeiculoSelecionado = await repositorioGrupoVeiculo.SelecionarPorIdAsync(request.GrupoVeiculoId);

            if (grupoVeiculoSelecionado is null)
                return Result.Fail(AluguelResultadosErro.GrupoVeiculoNullErro(request.GrupoVeiculoId));

            var veiculoSelecionado = await repositorioVeiculo.SelecionarPorIdAsync(request.VeiculoId);

            if (veiculoSelecionado is null)
                return Result.Fail(AluguelResultadosErro.VeiculoNullErro(request.VeiculoId));

            if (!grupoVeiculoSelecionado.Veiculos.Contains(veiculoSelecionado))
                return Result.Fail(AluguelResultadosErro.VeiculoNaoPertenceAoGrupoVeiculoErro(veiculoSelecionado.Modelo, grupoVeiculoSelecionado.Nome));

            var planoCobrancaSelecionado = await repositorioPlanoCobrnca.SelecionarPorIdAsync(request.PlanoCobrancaId);

            if (planoCobrancaSelecionado is null)
                return Result.Fail(AluguelResultadosErro.PlanoCobrancaNullErro(request.PlanoCobrancaId));

            if (planoCobrancaSelecionado.GrupoVeiculo.Id != grupoVeiculoSelecionado.Id)
                return Result.Fail(AluguelResultadosErro.PlanoCobrancaNaoPertenceAoGrupoVeiculoErro(planoCobrancaSelecionado.TipoPlano.ToString(), grupoVeiculoSelecionado.Nome));

            var taxasServicosSelecionados = new List<TaxaServico>();

            foreach (Guid id in request.TaxasServicosIds)
            {
                var taxaServico = await repositorioTaxaServico.SelecionarPorIdAsync(id);

                if (taxaServico is null)
                    return Result.Fail(AluguelResultadosErro.TaxaServicoNullErro(id));

                taxasServicosSelecionados.Add(taxaServico);
            }

            var aluguel = new Aluguel(
                condutorSelecionado,
                grupoVeiculoSelecionado,
                veiculoSelecionado,
                request.DataEntrada,
                request.DataRetorno,
                planoCobrancaSelecionado,
                taxasServicosSelecionados
                )
            {
                EmpresaId = tenantProvider.EmpresaId.GetValueOrDefault()
            };

            var resultadoValidacao = await validador.ValidateAsync(aluguel);

            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors
                   .Select(failure => failure.ErrorMessage)
                   .ToList();

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            var alugueis = await repositorioAluguel.SelecionarTodosAsync();

            if (alugueis.Any(x => x.Veiculo.Id == veiculoSelecionado.Id))
                return Result.Fail(AluguelResultadosErro.VeiculoJaSelecionadoErro(veiculoSelecionado.Modelo));


            await repositorioAluguel.InserirAsync(aluguel);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new InserirAluguelResponse(aluguel.Id));
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

    }
}