using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Editar;

public record EditarVeiculoPartialRequest(
    Guid GrupoVeiculoId,
    string Placa,
    string Modelo,
    string Marca,
    string Cor,
    TipoCombustivel TipoCombustivel,
    decimal CapacidadeTanque);

public record EditarVeiculoRequest(
    Guid Id,
    Guid GrupoVeiculoId,
    string Placa,
    string Modelo,
    string Marca,
    string Cor,
    TipoCombustivel TipoCombustivel,
    decimal CapacidadeTanque)
    : IRequest<Result<EditarVeiculoResponse>>;
