using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.Editar;

public record EditarTaxaServicoPartialRequest(
    string Nome,
    decimal Valor,
    TipoCobranca TipoCobranca
    );

public record EditarTaxaServicoRequest(
    Guid Id,
    string Nome,
    decimal Valor,
    TipoCobranca TipoCobranca
    ) : IRequest<Result<EditarTaxaServicoResponse>>;
