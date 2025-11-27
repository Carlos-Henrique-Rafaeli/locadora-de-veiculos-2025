using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarPorId;

internal class SelecionarPessoaFisicaPorIdRequestHandler(
    IRepositorioPessoaFisica repositorioPessoaFisica
) : IRequestHandler<SelecionarPessoaFisicaPorIdRequest, Result<SelecionarPessoaFisicaPorIdResponse>>
{
    public async Task<Result<SelecionarPessoaFisicaPorIdResponse>> Handle(SelecionarPessoaFisicaPorIdRequest request, CancellationToken cancellationToken)
    {
        var pessoaFisicaSelecionada = await repositorioPessoaFisica.SelecionarPorIdAsync(request.Id);

        if (pessoaFisicaSelecionada is null)
            return Result.Fail(ErrorResults.NotFoundError(request.Id));

        var resposta = new SelecionarPessoaFisicaPorIdResponse(
            new SelecionarPessoaFisicaDto(
                pessoaFisicaSelecionada.Id,
                pessoaFisicaSelecionada.Nome,
                pessoaFisicaSelecionada.Telefone,
                pessoaFisicaSelecionada.Endereco,
                pessoaFisicaSelecionada.Cpf,
                pessoaFisicaSelecionada.Rg,
                pessoaFisicaSelecionada.Cnh,
                pessoaFisicaSelecionada.PessoaJuridica is not null
                    ? new SelecionarPessoaJuridicaDto(
                        pessoaFisicaSelecionada.PessoaJuridica.Id,
                        pessoaFisicaSelecionada.PessoaJuridica.Nome,
                        pessoaFisicaSelecionada.PessoaJuridica.Telefone,
                        pessoaFisicaSelecionada.PessoaJuridica.Endereco,
                        pessoaFisicaSelecionada.PessoaJuridica.Cnpj,
                        new SelecionarCondutorDto(
                            pessoaFisicaSelecionada.PessoaJuridica.Condutor.Id,
                            pessoaFisicaSelecionada.PessoaJuridica.Condutor.Nome,
                            pessoaFisicaSelecionada.PessoaJuridica.Condutor.Email,
                            pessoaFisicaSelecionada.PessoaJuridica.Condutor.Cpf,
                            pessoaFisicaSelecionada.PessoaJuridica.Condutor.Cnh,
                            pessoaFisicaSelecionada.PessoaJuridica.Condutor.ValidadeCnh,
                            pessoaFisicaSelecionada.PessoaJuridica.Condutor.Telefone
                        )
                    )
                    : null
                ));

        return Result.Ok(resposta);
    }
}
