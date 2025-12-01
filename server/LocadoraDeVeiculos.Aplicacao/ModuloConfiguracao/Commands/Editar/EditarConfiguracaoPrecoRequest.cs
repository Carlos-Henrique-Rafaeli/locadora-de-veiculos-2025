using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloConfiguracao.Commands.Editar;

public record EditarConfiguracaoPrecoRequest(
    decimal Gasolina,
    decimal Diesel,
    decimal Etanol
    ) : IRequest<Result<EditarConfiguracaoPrecoResponse>>;
