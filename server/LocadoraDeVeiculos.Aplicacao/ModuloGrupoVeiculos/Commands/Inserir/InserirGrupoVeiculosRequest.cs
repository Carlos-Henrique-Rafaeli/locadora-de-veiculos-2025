using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands.Inserir;

public record InserirGrupoVeiculosRequest(string nome) 
    : IRequest<Result<InserirGrupoVeiculosResponse>>;
