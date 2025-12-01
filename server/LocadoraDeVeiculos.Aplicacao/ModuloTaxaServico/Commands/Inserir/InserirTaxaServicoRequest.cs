using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxaServico.Commands.Inserir;

public record InserirTaxaServicoRequest(
    string Nome,
    decimal Valor,
    TipoCobranca TipoCobranca
    ) 
    : IRequest<Result<InserirTaxaServicoResponse>>;


