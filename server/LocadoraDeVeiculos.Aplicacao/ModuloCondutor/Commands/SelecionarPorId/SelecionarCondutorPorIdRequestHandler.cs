using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.SelecionarTodos;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.SelecionarPorId;

internal class SelecionarCondutorPorIdRequestHandler(
    IRepositorioCondutor repositorioCondutor
) : IRequestHandler<SelecionarCondutorPorIdRequest, Result<SelecionarCondutorPorIdResponse>>
{
    public async Task<Result<SelecionarCondutorPorIdResponse>> Handle(SelecionarCondutorPorIdRequest request, CancellationToken cancellationToken)
    {
        var condutorSelecionado = await repositorioCondutor.SelecionarPorIdAsync(request.Id);

        if (condutorSelecionado is null)
            return Result.Fail(ErrorResults.NotFoundError(request.Id));

        var resposta = new SelecionarCondutorPorIdResponse(
            new SelecionarCondutoresDto(
                condutorSelecionado.Id,
                condutorSelecionado.Nome,
                condutorSelecionado.Email,
                condutorSelecionado.Cpf,
                condutorSelecionado.Cnh,
                condutorSelecionado.ValidadeCnh,
                condutorSelecionado.Telefone
                ));

        return Result.Ok(resposta);
    }
}