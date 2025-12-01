using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.SelecionarTodos;

public record SelecionarTaxasServicosRequest() : IRequest<Result<SelecionarTaxasServicosResponse>>;
