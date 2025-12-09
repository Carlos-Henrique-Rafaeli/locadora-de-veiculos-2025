using FluentValidation;

namespace LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;

public class ValidadorPlanoCobranca : AbstractValidator<PlanoCobranca>
{
    public ValidadorPlanoCobranca()
    {
        RuleFor(x => x.TipoPlano)
            .IsInEnum()
            .WithMessage("O {PropertyName} deve ser um valor válido.");

        RuleFor(x => x.GrupoVeiculo)
            .NotNull()
            .WithMessage("O campo {PropertyName} deve ser obrigatório.");

        When(x => x.TipoPlano == TipoPlano.PlanoDiario, () =>
        {
            RuleFor(x => x.ValorDiario)
            .NotNull().WithMessage("O campo {PropertyName} deve ser obrigatório.")
            .GreaterThan(0).WithMessage("O campo {PropertyName} deve ser maior que {ComparisonValue}.")
            .LessThan(decimal.MaxValue).WithMessage("O campo {PropertyName} deve ser maior que {ComparisonValue}.");

            RuleFor(x => x.ValorKm)
                .NotNull().WithMessage("O campo {PropertyName} deve ser obrigatório.")
                .GreaterThan(0).WithMessage("O campo {PropertyName} deve ser maior que {ComparisonValue}.")
                .LessThan(decimal.MaxValue).WithMessage("O campo {PropertyName} deve ser maior que {ComparisonValue}.");
        });

        When(x => x.TipoPlano == TipoPlano.PlanoControlado, () =>
        {
            RuleFor(x => x.ValorDiario)
                .NotNull().WithMessage("O campo {PropertyName} deve ser obrigatório.")
                .GreaterThan(0).WithMessage("O campo {PropertyName} deve ser maior que {ComparisonValue}.")
                .LessThan(decimal.MaxValue).WithMessage("O campo {PropertyName} deve ser maior que {ComparisonValue}.");


            RuleFor(x => x.KmIncluso)
               .NotNull().WithMessage("O campo {PropertyName} deve ser obrigatório.")
               .GreaterThanOrEqualTo(0).WithMessage("O campo {PropertyName} deve ser maior ou igual a que {ComparisonValue}.")
               .LessThan(int.MaxValue).WithMessage("O campo {PropertyName} deve ser maior que {ComparisonValue}.");


            RuleFor(x => x.ValorKmExcedente)
                .NotNull().WithMessage("O campo {PropertyName} deve ser obrigatório.")  
                .GreaterThan(0).WithMessage("O campo {PropertyName} deve ser maior que {ComparisonValue}.")
                .LessThan(decimal.MaxValue).WithMessage("O campo {PropertyName} deve ser maior que {ComparisonValue}.");

        });

        When(x => x.TipoPlano == TipoPlano.PlanoLivre, () =>
        {
            RuleFor(x => x.ValorFixo)
                .NotNull().WithMessage("O campo {PropertyName} deve ser obrigatório.")
                .GreaterThan(0).WithMessage("O campo {PropertyName} deve ser maior ou igual a {ComparisonValue}.")
                .LessThan(decimal.MaxValue).WithMessage("O campo {PropertyName} deve ser maior que {ComparisonValue}.");

        });

        
    }
}
