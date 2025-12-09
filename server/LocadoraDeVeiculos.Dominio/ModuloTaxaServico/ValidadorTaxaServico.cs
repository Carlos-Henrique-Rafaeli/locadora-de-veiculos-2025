using FluentValidation;

namespace LocadoraDeVeiculos.Dominio.ModuloTaxaServico;

public class ValidadorTaxaServico : AbstractValidator<TaxaServico>
{
    public ValidadorTaxaServico()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .MinimumLength(3).WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres.")
            .MaximumLength(100).WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres.");

        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("O campo {PropertyName} deve ser maior que zero.")
            .LessThan(decimal.MaxValue).WithMessage("O campo {PropertyName} deve ser maior que {ComparisonValue}.");

        RuleFor(x => x.TipoCobranca)
            .IsInEnum()
            .WithMessage("O {PropertyName} deve ser um valor válido.");
    }
}
