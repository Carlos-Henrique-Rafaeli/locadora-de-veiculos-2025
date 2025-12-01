using FluentValidation;

namespace LocadoraDeVeiculos.Dominio.ModuloAluguel;

public class ValidadorAluguel : AbstractValidator<Aluguel>
{
    public ValidadorAluguel()
    {
        RuleFor(x => x.Condutor)
            .NotNull()
            .WithMessage("O campo {PropertyName} deve ser obrigatório.");

        RuleFor(x => x.GrupoVeiculo)
            .NotNull()
            .WithMessage("O campo {PropertyName} deve ser obrigatório.");

        RuleFor(x => x.Veiculo)
            .NotNull()
            .WithMessage("O campo {PropertyName} deve ser obrigatório.");

        RuleFor(x => x.DataEntrada)
            .NotEmpty().WithMessage("O campo {PropertyName} deve ser obrigatório.")
            .LessThan(x => x.DataRetorno)
            .WithMessage("A {PropertyName} deve ser anterior à Data de Retorno.");

        RuleFor(x => x.DataRetorno)
            .NotEmpty().WithMessage("O campo {PropertyName} deve ser obrigatório.")
            .GreaterThan(x => x.DataEntrada)
            .WithMessage("A {PropertyName} deve ser posterior à Data de Entrada.");

        RuleFor(x => x.PlanoCobranca)
            .NotNull()
            .WithMessage("O campo {PropertyName} deve ser obrigatório.");


    }
}
