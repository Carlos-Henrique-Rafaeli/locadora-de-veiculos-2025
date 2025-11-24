using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.Editar;

public record EditarCondutorPartialRequest(
    string Nome,
    string Email,
    string Cpf,
    string Cnh,
    DateTime ValidadeCnh,
    string Telefone
    );

public record EditarCondutorRequest(
    Guid Id,
    string Nome,
    string Email,
    string Cpf,
    string Cnh,
    DateTime ValidadeCnh,
    string Telefone
    ) : IRequest<Result<EditarCondutorResponse>>;
