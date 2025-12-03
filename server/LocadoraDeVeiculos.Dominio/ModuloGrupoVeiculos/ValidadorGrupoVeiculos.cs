
using FluentValidation;

namespace LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;

public class ValidadorGrupoVeiculos : AbstractValidator<GrupoVeiculo>
{
    public ValidadorGrupoVeiculos()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Nome).MinimumLength(3)
                .WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres");

                RuleFor(x => x.Nome).MaximumLength(100)
                .WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres");
            });
    }
}
