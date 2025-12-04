using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.Editar;

internal class EditarFuncionarioRequestHandler(
    LocadoraDeVeiculosDbContext appDbContext,
    IRepositorioFuncionario repositorioFuncionario,
    ITenantProvider tenantProvider,
    IValidator<EditarFuncionarioRequest> validator,
    ILogger<EditarFuncionarioRequestHandler> logger
) : IRequestHandler<EditarFuncionarioRequest, Result<EditarFuncionarioResponse>>
{
    public async Task<Result<EditarFuncionarioResponse>> Handle(
        EditarFuncionarioRequest command, CancellationToken cancellationToken)
    {
        var registroEncontrado = await repositorioFuncionario.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        var registros = await repositorioFuncionario.SelecionarTodosAsync();

        if (registros.Any(x => !x.Id.Equals(command.Id) && x.Cpf.Equals(command.Cpf)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Um funcionário com este CPF já está cadastrado."));

        try
        {
            Funcionario funcionarioEditado = new(
                registroEncontrado.UsuarioId,
                tenantProvider.EmpresaId.GetValueOrDefault(),
                command.NomeCompleto,
                command.Cpf,
                registroEncontrado.Email,
                command.Salario,
                command.AdmissaoEmUtc
            );

            await repositorioFuncionario.EditarAsync(command.Id, funcionarioEditado);

            await appDbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(new EditarFuncionarioResponse(command.Id));
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Command}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}