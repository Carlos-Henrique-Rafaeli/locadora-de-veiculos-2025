using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.Inserir;

internal class InserirFuncionarioRequestHandler(
    LocadoraDeVeiculosDbContext appDbContext,
    IRepositorioFuncionario repositorioFuncionario,
    UserManager<Usuario> userManager,
    RoleManager<Cargo> roleManager,
    ITenantProvider tenantProvider,
    IValidator<InserirFuncionarioRequest> validator,
    ILogger<InserirFuncionarioRequestHandler> logger
) : IRequestHandler<InserirFuncionarioRequest, Result<InserirFuncionarioResponse>>
{
    public async Task<Result<InserirFuncionarioResponse>> Handle(
        InserirFuncionarioRequest request, CancellationToken cancellationToken)
    {
        List<Funcionario> registros = await repositorioFuncionario.SelecionarTodosAsync();

        if (registros.Any(x => x.Cpf.Equals(request.Cpf)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Um funcionário com este CPF já está cadastrado."));

        var resultadoValidacao = await validator.ValidateAsync(request, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        Usuario? usuario = null;

        try
        {
            usuario = new Usuario()
            {
                FullName = request.NomeCompleto,
                UserName = request.Email,
                Email = request.Email
            };

            var resultadoCriacaoUsuario =
                await userManager.CreateAsync(usuario, request.ConfirmarSenha);

            if (!resultadoCriacaoUsuario.Succeeded)
            {
                var erros = resultadoCriacaoUsuario.Errors.Select(e => e.Description);

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            var cargoStr = CargoUsuario.Funcionario.ToString();

            var resultadoBuscaCargo = await roleManager.FindByNameAsync(cargoStr);

            if (resultadoBuscaCargo is null)
            {
                var cargo = new Cargo()
                {
                    Name = cargoStr,
                    NormalizedName = cargoStr.ToUpperInvariant(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };

                await roleManager.CreateAsync(cargo);
            }

            var resultadoInclusaoCargo = await userManager.AddToRoleAsync(usuario, cargoStr);

            if (!resultadoInclusaoCargo.Succeeded)
            {
                var erros = resultadoInclusaoCargo.Errors.Select(e => e.Description);

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            var funcionario = new Funcionario(
               usuario.Id,
               tenantProvider.EmpresaId.GetValueOrDefault(),
               request.NomeCompleto,
               request.Cpf,
               request.Email,
               request.Salario,
               request.AdmissaoEmUtc
            );

            await repositorioFuncionario.InserirAsync(funcionario);

            await appDbContext.SaveChangesAsync(cancellationToken);

            var resultado = new InserirFuncionarioResponse(funcionario.Id);

            return Result.Ok(resultado);
        }
        catch (Exception ex)
        {
            if (usuario is not null)
                await userManager.DeleteAsync(usuario);

            logger.LogError(
                ex,
                "Ocorreu um erro durante o cadastro de {@Request}.",
                request
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}