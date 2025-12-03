using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Editar;

internal class EditarAluguelRequestHandler(
    IRepositorioAluguel repositorioAluguel,
    IRepositorioCondutor repositorioCondutor,
    IRepositorioGrupoVeiculos repositorioGrupoVeiculo,
    IRepositorioVeiculo repositorioVeiculo,
    IRepositorioPlanoCobranca repositorioPlanoCobrnca,
    IRepositorioTaxaServico repositorioTaxaServico,
    IContextoPersistencia contexto,
    IValidator<Aluguel> validador
) : IRequestHandler<EditarAluguelRequest, Result<EditarAluguelResponse>>
{
    public async Task<Result<EditarAluguelResponse>> Handle(EditarAluguelRequest request, CancellationToken cancellationToken)
    {
        var aluguelSelecionado = await repositorioAluguel.SelecionarPorIdAsync(request.Id);

        if (aluguelSelecionado == null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        if (!aluguelSelecionado.EstaAberto)
            return Result.Fail(AluguelErrorResults.AluguelFechadoError(aluguelSelecionado.Id));

        var condutorSelecionado = await repositorioCondutor.SelecionarPorIdAsync(request.CondutorId);

        if (condutorSelecionado is null)
            return Result.Fail(AluguelErrorResults.CondutorNullError(request.CondutorId));

        if (condutorSelecionado.ValidadeCnh < DateTime.Today)
            return Result.Fail(AluguelErrorResults.ValidadeCnhVencidaError(condutorSelecionado.Cpf));

        var grupoVeiculoSelecionado = await repositorioGrupoVeiculo.SelecionarPorIdAsync(request.GrupoVeiculoId);

        if (grupoVeiculoSelecionado is null)
            return Result.Fail(AluguelErrorResults.GrupoVeiculoNullError(request.GrupoVeiculoId));

        var veiculoSelecionado = await repositorioVeiculo.SelecionarPorIdAsync(request.VeiculoId);

        if (veiculoSelecionado is null)
            return Result.Fail(AluguelErrorResults.VeiculoNullError(request.VeiculoId));

        if (!grupoVeiculoSelecionado.Veiculos.Contains(veiculoSelecionado))
            return Result.Fail(AluguelErrorResults.VeiculoNaoPertenceAoGrupoVeiculoError(veiculoSelecionado.Modelo, grupoVeiculoSelecionado.Nome));

        var planoCobrancaSelecionado = await repositorioPlanoCobrnca.SelecionarPorIdAsync(request.PlanoCobrancaId);

        if (planoCobrancaSelecionado is null)
            return Result.Fail(AluguelErrorResults.PlanoCobrancaNullError(request.PlanoCobrancaId));

        if (planoCobrancaSelecionado.GrupoVeiculo.Id != grupoVeiculoSelecionado.Id)
            return Result.Fail(AluguelErrorResults.PlanoCobrancaNaoPertenceAoGrupoVeiculoError(planoCobrancaSelecionado.TipoPlano.ToString(), grupoVeiculoSelecionado.Nome));

        var taxasServicosSelecionados = new List<TaxaServico>();

        foreach (Guid id in request.TaxasServicosIds)
        {
            var taxaServico = await repositorioTaxaServico.SelecionarPorIdAsync(id);

            if (taxaServico is null)
                return Result.Fail(AluguelErrorResults.TaxaServicoNullError(id));

            taxasServicosSelecionados.Add(taxaServico);
        }

        var alugueis = await repositorioAluguel.SelecionarTodosAsync();

        if (alugueis.Where(a => a.Id != aluguelSelecionado.Id).Any(x => x.Veiculo.Id == veiculoSelecionado.Id))
            return Result.Fail(AluguelErrorResults.VeiculoJaSelecionadoError(veiculoSelecionado.Modelo));

        aluguelSelecionado.Condutor = condutorSelecionado;
        aluguelSelecionado.GrupoVeiculo = grupoVeiculoSelecionado;
        aluguelSelecionado.Veiculo = veiculoSelecionado;
        aluguelSelecionado.DataEntrada = request.DataEntrada;
        aluguelSelecionado.DataRetorno = request.DataRetorno;
        aluguelSelecionado.PlanoCobranca = planoCobrancaSelecionado;
        aluguelSelecionado.TaxasServicos = taxasServicosSelecionados;

        var resultadoValidacao =
            await validador.ValidateAsync(aluguelSelecionado, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        var grupoVeiculos = await repositorioAluguel.SelecionarTodosAsync();

        try
        {
            await repositorioAluguel.EditarAsync(aluguelSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new EditarAluguelResponse(aluguelSelecionado.Id));
    }
}
