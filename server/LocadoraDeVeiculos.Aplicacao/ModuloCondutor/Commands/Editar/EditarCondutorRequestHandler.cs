using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.Editar;

internal class EditarCondutorRequestHandler(
    IRepositorioCondutor repositorioCondutor,
    IRepositorioCliente repositorioCliente,
    LocadoraDeVeiculosDbContext contexto,
    IValidator<Condutor> validador
) : IRequestHandler<EditarCondutorRequest, Result<EditarCondutorResponse>>
{
    public async Task<Result<EditarCondutorResponse>> Handle(EditarCondutorRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var condutorSelecionado = await repositorioCondutor.SelecionarPorIdAsync(request.Id);

            if (condutorSelecionado == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

            var clienteSelecionado = await repositorioCliente.SelecionarPorIdAsync(request.ClienteId);

            if (clienteSelecionado is null)
                return Result.Fail(CondutorResultadosErro.ClienteNullErro(request.ClienteId));

            var condutorNovo = new Condutor(
                clienteSelecionado,
                request.ClienteCondutor,
                request.Nome,
                request.Email,
                request.Cpf,
                request.Cnh,
                request.ValidadeCnh,
                request.Telefone
            );

            var resultadoValidacao =
                await validador.ValidateAsync(condutorNovo, cancellationToken);

            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors
                    .Select(failure => failure.ErrorMessage)
                    .ToList();

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            var condutoresRegistrados = await repositorioCondutor.SelecionarTodosAsync();

            if (CpfDuplicado(condutorNovo, condutoresRegistrados, request.Id))
                return Result.Fail(CondutorResultadosErro.CpfDuplicadoErro(condutorNovo.Nome));

            if (CnhDuplicada(condutorNovo, condutoresRegistrados, request.Id))
                return Result.Fail(CondutorResultadosErro.CnhDuplicadaErro(condutorNovo.Nome));

            if (CnhVencida(condutorNovo))
                return Result.Fail(CondutorResultadosErro.CnhVencidaErro(condutorNovo.Nome));

            await repositorioCondutor.EditarAsync(request.Id, condutorNovo);

            await contexto.SaveChangesAsync(cancellationToken);

            return Result.Ok(new EditarCondutorResponse(condutorSelecionado.Id));
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    private bool CpfDuplicado(Condutor condutor, IList<Condutor> condutores, Guid condutorAntigo)
    {
        return condutores
            .Where(r => r.Id != condutorAntigo)
            .Any(registro => string.Equals(
                registro.Cpf,
                condutor.Cpf,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }

    private bool CnhDuplicada(Condutor condutor, IList<Condutor> condutores, Guid condutorAntigo)
    {
        return condutores
            .Where(r => r.Id != condutorAntigo)
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
