using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands.Editar;

public record EditarCondutorPartialRequest(
    Guid ClienteId,
    bool ClienteCondutor,
    string Nome,
    string Email,
    string Cpf,
    string Cnh,
    DateTime ValidadeCnh,
    string Telefone
    );

public record EditarCondutorRequest(
    Guid Id,
    Guid ClienteId,
    bool ClienteCondutor,
    string Nome,
    string Email,
    string Cpf,
    string Cnh,
    DateTime ValidadeCnh,
    string Telefone
    ) : IRequest<Result<EditarCondutorResponse>>;
