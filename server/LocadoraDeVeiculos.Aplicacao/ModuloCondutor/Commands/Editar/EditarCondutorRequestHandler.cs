using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.Editar;

internal class EditarCondutorRequestHandler(
    IRepositorioCondutor repositorioCondutor,
    IRepositorioCliente repositorioCliente,
    IContextoPersistencia contexto,
    IValidator<Condutor> validador
) : IRequestHandler<EditarCondutorRequest, Result<EditarCondutorResponse>>
{
    public async Task<Result<EditarCondutorResponse>> Handle(EditarCondutorRequest request, CancellationToken cancellationToken)
    {
        var condutorSelecionado = await repositorioCondutor.SelecionarPorIdAsync(request.Id);

        if (condutorSelecionado == null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(request.Id));

        var clienteSelecionado = await repositorioCliente.SelecionarPorIdAsync(request.ClienteId);

        if (clienteSelecionado is null)
            return Result.Fail(CondutorErrorResults.ClienteNullError(request.ClienteId));

        condutorSelecionado.Cliente = clienteSelecionado;
        condutorSelecionado.ClienteCondutor = request.ClienteCondutor;
        condutorSelecionado.Nome = request.Nome;
        condutorSelecionado.Email = request.Email;
        condutorSelecionado.Cpf = request.Cpf;
        condutorSelecionado.Cnh = request.Cnh;
        condutorSelecionado.ValidadeCnh = request.ValidadeCnh;
        condutorSelecionado.Telefone = request.Telefone;

        var resultadoValidacao =
            await validador.ValidateAsync(condutorSelecionado, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
                .Select(failure => failure.ErrorMessage)
                .ToList();

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        var condutoresRegistrados = await repositorioCondutor.SelecionarTodosAsync();

        if (CpfDuplicado(condutorSelecionado, condutoresRegistrados))
            return Result.Fail(CondutorErrorResults.CpfDuplicado(condutorSelecionado.Nome));

        if (CnhDuplicada(condutorSelecionado, condutoresRegistrados))
            return Result.Fail(CondutorErrorResults.CnhDuplicada(condutorSelecionado.Nome));

        if (CnhVencida(condutorSelecionado))
            return Result.Fail(CondutorErrorResults.CnhVencida(condutorSelecionado.Nome));

        try
        {
            await repositorioCondutor.EditarAsync(condutorSelecionado);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }

        return Result.Ok(new EditarCondutorResponse(condutorSelecionado.Id));
    }

    private bool CpfDuplicado(Condutor condutor, IList<Condutor> condutores)
    {
        return condutores
            .Where(r => r.Id != condutor.Id)
            .Any(registro => string.Equals(
                registro.Cpf,
                condutor.Cpf,
                StringComparison.CurrentCultureIgnoreCase)
            );
    }

    private bool CnhDuplicada(Condutor condutor, IList<Condutor> condutores)
    {
        return condutores
            .Where(r => r.Id != condutor.Id)
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
