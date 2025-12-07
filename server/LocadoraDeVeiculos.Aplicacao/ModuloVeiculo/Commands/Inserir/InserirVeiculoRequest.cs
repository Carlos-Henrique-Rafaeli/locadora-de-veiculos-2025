using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Inserir;

public record InserirVeiculoRequest(
    Guid GrupoVeiculoId,
    string Placa,
    string Modelo,
    string Marca,
    string Cor,
    TipoCombustivel TipoCombustivel,
    decimal CapacidadeTanque,
    IFormFile? Imagem
) : IRequest<Result<InserirVeiculoResponse>>;
