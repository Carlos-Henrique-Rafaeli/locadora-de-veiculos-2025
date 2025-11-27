using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands.SelecionarPorId;

internal class SelecionarPessoaJuridicaPorIdRequestHandler(
    IRepositorioPessoaJuridica repositorioPessoaJuridica
) : IRequestHandler<SelecionarPessoaJuridicaPorIdRequest, Result<SelecionarPessoaJuridicaPorIdResponse>>
{
    public async Task<Result<SelecionarPessoaJuridicaPorIdResponse>> Handle(SelecionarPessoaJuridicaPorIdRequest request, CancellationToken cancellationToken)
    {
        var pessoaJuridicaSelecionada = await repositorioPessoaJuridica.SelecionarPorIdAsync(request.Id);

        if (pessoaJuridicaSelecionada is null)
            return Result.Fail(ErrorResults.NotFoundError(request.Id));

        var resposta = new SelecionarPessoaJuridicaPorIdResponse(
            new SelecionarPessoaJuridicaDto(
                pessoaJuridicaSelecionada.Id,
                pessoaJuridicaSelecionada.Nome,
                pessoaJuridicaSelecionada.Telefone,
                pessoaJuridicaSelecionada.Endereco,
                pessoaJuridicaSelecionada.Cnpj,
                new SelecionarCondutorDto(
                    pessoaJuridicaSelecionada.Condutor.Id,
                    pessoaJuridicaSelecionada.Condutor.Nome,
                    pessoaJuridicaSelecionada.Condutor.Email,
                    pessoaJuridicaSelecionada.Condutor.Cpf,
                    pessoaJuridicaSelecionada.Condutor.Cnh,
                    pessoaJuridicaSelecionada.Condutor.ValidadeCnh,
                    pessoaJuridicaSelecionada.Condutor.Telefone
                )
            ));

        return Result.Ok(resposta);
    }
}
