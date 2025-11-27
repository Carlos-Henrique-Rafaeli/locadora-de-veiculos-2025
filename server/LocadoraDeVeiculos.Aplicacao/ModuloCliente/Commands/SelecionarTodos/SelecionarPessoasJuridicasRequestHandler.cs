using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;

internal class SelecionarPessoasJuridicasRequestHandler(
    IRepositorioPessoaJuridica repositorioPessoaJuridica
) : IRequestHandler<SelecionarPessoasJuridicasRequest, Result<SelecionarPessoasJuridicasResponse>>
{
    public async Task<Result<SelecionarPessoasJuridicasResponse>> Handle(
        SelecionarPessoasJuridicasRequest request, CancellationToken cancellationToken)
    {
        var registros = await repositorioPessoaJuridica.SelecionarTodosAsync();

        var response = new SelecionarPessoasJuridicasResponse
        {
            QuantidadeRegistros = registros.Count(),
            Registros = registros.Select(x => new SelecionarPessoaJuridicaDto(
                x.Id,
                x.Nome,
                x.Telefone,
                x.Endereco,
                x.Cnpj,
                new SelecionarCondutorDto(
                    x.Condutor.Id,
                    x.Condutor.Nome,
                    x.Condutor.Email,
                    x.Condutor.Cpf,
                    x.Condutor.Cnh,
                    x.Condutor.ValidadeCnh,
                    x.Condutor.Telefone
                )
            ))
        };

        return Result.Ok(response);
    }
}