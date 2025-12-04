using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Infraestrutura.Jwt.Services;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.Excluir;

internal class ExcluirFuncionarioRequestHandler(
    LocadoraDeVeiculosDbContext dbContext,
    IRepositorioFuncionario repositorioFuncionario,
    RefreshTokenProvider refreshTokenProvider,
    UserManager<Usuario> userManager,
    ILogger<ExcluirFuncionarioRequestHandler> logger
) : IRequestHandler<ExcluirFuncionarioRequest, Result<ExcluirFuncionarioResponse>>
{
    public async Task<Result<ExcluirFuncionarioResponse>> Handle(
        ExcluirFuncionarioRequest command, CancellationToken cancellationToken)
    {
        try
        {
            Funcionario? funcionario = await repositorioFuncionario.SelecionarPorIdAsync(command.Id);

            if (funcionario is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

            Usuario? usuario = await userManager.FindByIdAsync(funcionario.UsuarioId.ToString());

            if (usuario is not null)
                await refreshTokenProvider.RevogarTokensUsuarioAsync(usuario.Id, "Exclusão");

            await repositorioFuncionario.ExcluirAsync(funcionario.Id);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(new ExcluirFuncionarioResponse());
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a exclusão de de {@Command}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}