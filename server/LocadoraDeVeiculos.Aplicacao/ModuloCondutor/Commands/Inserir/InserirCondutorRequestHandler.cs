using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.Inserir;

internal class InserirCondutorRequestHandler(
IContextoPersistencia contexto,
IRepositorioCondutor repositorioCondutor,
ITenantProvider tenantProvider,
IValidator<Condutor> validador
) : IRequestHandler<InserirCondutorRequest, Result<InserirCondutorResponse>>
{
    public async Task<Result<InserirCondutorResponse>> Handle(
        InserirCondutorRequest request, CancellationToken cancellationToken)
    {
        var condutor = new Condutor(
            request.Nome,
            request.Email,
            request.Cpf,
            request.Cnh,
            request.ValidadeCnh,
            request.Telefone
            )
        {
            UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault()
        };

        // validações
        var resultadoValidacao = await validador.ValidateAsync(condutor);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors
               .Select(failure => failure.ErrorMessage)
               .ToList();

            return Result.Fail(ErrorResults.BadRequestError(erros));
        }

        var CondutoresRegistrados = await repositorioCondutor.SelecionarTodosAsync();

        if (CpfDuplicado(condutor, CondutoresRegistrados))
            return Result.Fail(CondutorErrorResults.CpfDuplicado(condutor.Nome));

        if (CnhDuplicada(condutor, CondutoresRegistrados))
            return Result.Fail(CondutorErrorResults.CnhDuplicada(condutor.Nome));

        if (CnhVencida(condutor))
            return Result.Fail(CondutorErrorResults.CnhVencida(condutor.Nome));

        // inserção
        try
        {
            await repositorioCondutor.InserirAsync(condutor);

            await contexto.GravarAsync();
        }
        catch (Exception ex)
        {
            await contexto.RollbackAsync();

            return Result.Fail(ErrorResults.InternalServerError(ex));
        }

        return Result.Ok(new InserirCondutorResponse(condutor.Id));
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
