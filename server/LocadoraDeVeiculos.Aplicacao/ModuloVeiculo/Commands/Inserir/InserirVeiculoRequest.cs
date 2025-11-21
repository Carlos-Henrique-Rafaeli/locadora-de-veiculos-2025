using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands.Inserir;

public record InserirVeiculoRequest(
        Guid GrupoVeiculoId,
        string Placa,
        string Modelo,
        string Marca,
        string Cor,
        TipoCombustivel TipoCombustivel,
        decimal CapacidadeTanque) : IRequest<Result<InserirVeiculoResponse>>;
