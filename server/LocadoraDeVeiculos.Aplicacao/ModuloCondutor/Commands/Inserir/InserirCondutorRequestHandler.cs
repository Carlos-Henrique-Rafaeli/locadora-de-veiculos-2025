using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.Inserir;

internal class InserirCondutorRequestHandler(
    LocadoraDeVeiculosDbContext contexto,
    IRepositorioCondutor repositorioCondutor,
    IRepositorioCliente repositorioCliente,
    ITenantProvider tenantProvider,
    IValidator<Condutor> validador
    ) : IRequestHandler<InserirCondutorRequest, Result<InserirCondutorResponse>>
{
    public async Task<Result<InserirCondutorResponse>> Handle(
        InserirCondutorRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var clienteSelecionado = await repositorioCliente.SelecionarPorIdAsync(request.ClienteId);

            if (clienteSelecionado is null)
                return Result.Fail(CondutorResultadosErro.ClienteNullErro(request.ClienteId));

            var condutor = new Condutor(
                clienteSelecionado,
                request.ClienteCondutor,
                request.Nome,
                request.Email,
                request.Cpf,
                request.Cnh,
                request.ValidadeCnh,
                request.Telefone
                )
            {
                EmpresaId = tenantProvider.EmpresaId.GetValueOrDefault()
            };

            var resultadoValidacao = await validador.ValidateAsync(condutor);

            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors
                   .Select(failure => failure.ErrorMessage)
                   .ToList();

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            var condutoresRegistrados = await repositorioCondutor.SelecionarTodosAsync();

            if (CpfDuplicado(condutor, condutoresRegistrados))
                return Result.Fail(CondutorResultadosErro.CpfDuplicadoErro(condutor.Nome));

            if (CnhDuplicada(condutor, condutoresRegistrados))
                return Result.Fail(CondutorResultadosErro.CnhDuplicadaErro(condutor.Nome));

            if (CnhVencida(condutor))
                return Result.Fail(CondutorResultadosErro.CnhVencidaErro(condutor.Nome));

            await repositorioCondutor.InserirAsync(condutor);

            await contexto.SaveChangesAsync(cancellationToken);
         
            return Result.Ok(new InserirCondutorResponse(condutor.Id));
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    private bool CpfDuplicado(Condutor condutor, IList<Condutor> condutores)
    {
        return condutores
            .Any(registro => string.Equals(
                registro.Cpf,
                condutor.Cpf,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }

    private bool CnhDuplicada(Condutor condutor, IList<Condutor> condutores)
    {
        return condutores
            .Any(registro => string.Equals(
                registro.Cnh,
                condutor.Cnh,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }

    private bool CnhVencida(Condutor condutor)
    {
        return condutor.ValidadeCnh < DateTime.Now;
    }
}
