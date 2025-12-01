using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.Excluir;

public record ExcluirTaxaServicoRequest(Guid Id) : IRequest<Result<ExcluirTaxaServicoResponse>>;
