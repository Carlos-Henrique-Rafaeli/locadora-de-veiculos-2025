using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;

internal class SelecionarPessoasFisicasRequestHandler(
    IRepositorioPessoaFisica repositorioPessoaFisica
) : IRequestHandler<SelecionarPessoasFisicasRequest, Result<SelecionarPessoasFisicasResponse>>
{
    public async Task<Result<SelecionarPessoasFisicasResponse>> Handle(
        SelecionarPessoasFisicasRequest request, CancellationToken cancellationToken)
    {
        var registros = await repositorioPessoaFisica.SelecionarTodosAsync();

        var response = new SelecionarPessoasFisicasResponse
        {
            QuantidadeRegistros = registros.Count(),
            Registros = registros.Select(x => new SelecionarPessoaFisicaDto(
                x.Id,
                x.Nome,
                x.Telefone,
                x.Endereco,
                x.Cpf,
                x.Rg,
                x.Cnh,
                x.PessoaJuridica is not null
                    ? new SelecionarPessoaJuridicaDto(
                        x.PessoaJuridica.Id,
                        x.PessoaJuridica.Nome,
                        x.PessoaJuridica.Telefone,
                        x.PessoaJuridica.Endereco,
                        x.PessoaJuridica.Cnpj,
                        new SelecionarCondutorDto(
                            x.PessoaJuridica.Condutor.Id,
                            x.PessoaJuridica.Condutor.Nome,
                            x.PessoaJuridica.Condutor.Email,
                            x.PessoaJuridica.Condutor.Cpf,
                            x.PessoaJuridica.Condutor.Cnh,
                            x.PessoaJuridica.Condutor.ValidadeCnh,
                            x.PessoaJuridica.Condutor.Telefone
                        )
                    )
                    : null
                ))
        };

        return Result.Ok(response);
    }
}