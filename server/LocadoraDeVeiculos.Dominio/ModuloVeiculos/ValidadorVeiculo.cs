using FluentValidation;

namespace LocadoraDeVeiculos.Dominio.ModuloVeiculos;

public class ValidadorVeiculo : AbstractValidator<Veiculo>
{
    public ValidadorVeiculo()
    {
        RuleFor(x => x.Placa)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Matches(@"^(?:[A-Z]{3}-\d{4}|[A-Z]{3}\d[A-Z]\d{2})$").WithMessage("O campo {PropertyName} deve seguir o formato AAA-0000 ou BRA0S17.");

        RuleFor(x => x.Marca)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Marca).MinimumLength(3)
                .WithMessage("O campo {PropertyName} deve conter no mínimo {MinLenght} caracteres");

                RuleFor(x => x.Marca).MaximumLength(50)
                .WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres");
            });

        RuleFor(x => x.Modelo)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Modelo).MinimumLength(3)
                .WithMessage("O campo {PropertyName} deve conter no mínimo {MinLenght} caracteres");

                RuleFor(x => x.Modelo).MaximumLength(50)
                .WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres");
            });

        RuleFor(x => x.Cor)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Cor).MinimumLength(3)
                .WithMessage("O campo {PropertyName} deve conter no mínimo {MinLenght} caracteres");

                RuleFor(x => x.Cor).MaximumLength(50)
                .WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres");
            });

        RuleFor(x => x.CapacidadeTanque)
            .GreaterThan(0).WithMessage("O campo {PropertyName} deve ser maior que {ComparisonValue}.");

        RuleFor(x => x.TipoCombustivel)
            .IsInEnum().WithMessage("O campo {PropertyName} possui um valor inválido.");

        RuleFor(x => x.GrupoVeiculo)
            .NotNull().WithMessage("O campo {PropertyName} é obrigatório.");
    }
}
