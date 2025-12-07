using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Editar;

public record EditarVeiculoPartialRequest(
    Guid GrupoVeiculoId,
    string Placa,
    string Modelo,
    string Marca,
    string Cor,
    TipoCombustivel TipoCombustivel,
    decimal CapacidadeTanque,
    IFormFile? Imagem
);

public record EditarVeiculoRequest(
    Guid Id,
    Guid GrupoVeiculoId,
    string Placa,
    string Modelo,
    string Marca,
    string Cor,
    TipoCombustivel TipoCombustivel,
    decimal CapacidadeTanque,
    IFormFile? Imagem
) : IRequest<Result<EditarVeiculoResponse>>;
