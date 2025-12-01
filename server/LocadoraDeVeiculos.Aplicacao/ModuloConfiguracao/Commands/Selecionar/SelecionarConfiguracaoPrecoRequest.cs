using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloConfiguracao.Commands.Selecionar;

public record SelecionarConfiguracaoPrecoRequest 
    : IRequest<Result<SelecionarConfiguracaoPrecoResponse>>;
