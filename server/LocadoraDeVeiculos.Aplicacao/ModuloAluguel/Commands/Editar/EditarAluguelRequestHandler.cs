using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Editar;

internal class EditarAluguelRequestHandler(
    IRepositorioAluguel repositorioAluguel,
    IRepositorioCondutor repositorioCondutor,
    IRepositorioGrupoVeiculos repositorioGrupoVeiculo,
    IRepositorioVeiculo repositorioVeiculo,
    IRepositorioPlanoCobranca repositorioPlanoCobrnca,
    IRepositorioTaxaServico repositorioTaxaServico,
    LocadoraDeVeiculosDbContext contexto,
    IValidator<Aluguel> validador
) : IRequestHandler<EditarAluguelRequest, Result<EditarAluguelResponse>>
{
    public async Task<Result<EditarAluguelResponse>> Handle(EditarAluguelRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var aluguelSelecionado = await repositorioAluguel.SelecionarPorIdAsync(request.Id);

            if (aluguelSelecionado == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            if (!aluguelSelecionado.EstaAberto)
                return Result.Fail(AluguelResultadosErro.AluguelFechadoErro(aluguelSelecionado.Id));

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

            var alugueis = await repositorioAluguel.SelecionarTodosAsync();

            if (alugueis.Where(a => a.Id != aluguelSelecionado.Id).Any(x => x.EstaAberto && x.Veiculo.Id == veiculoSelecionado.Id))
                return Result.Fail(AluguelResultadosErro.VeiculoJaSelecionadoErro(veiculoSelecionado.Modelo));

            var resultadoValidacao =
                await validador.ValidateAsync(aluguelSelecionado, cancellationToken);

            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors
                    .Select(failure => failure.ErrorMessage)
                    .ToList();

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            var novoAluguel = new Aluguel(
                condutorSelecionado,
                grupoVeiculoSelecionado,
                veiculoSelecionado,
                request.DataEntrada,
                request.DataRetorno,
                planoCobrancaSelecionado,
                taxasServicosSelecionados
            );

            await repositorioAluguel.EditarAsync(request.Id, novoAluguel);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new EditarAluguelResponse(aluguelSelecionado.Id));
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
