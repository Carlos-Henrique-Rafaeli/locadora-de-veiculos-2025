using FluentValidation;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Finalizar;

public class FinalizarAluguelValidador : AbstractValidator<FinalizarAluguelRequest>
{
    public FinalizarAluguelValidador()
    {
        RuleFor(x => x.kmInicial)
            .GreaterThan(0).WithMessage("O campo {PropertyName} deve ser maior que zero.")
            .LessThan(x => x.kmAtual).WithMessage("O campo {PropertyName} deve ser menor que a quilometragem atual.")
            .LessThan(x => decimal.MaxValue).WithMessage("O campo {PropertyName} deve ser menor que {ComparisonValue}.");

        RuleFor(x => x.kmAtual)
            .GreaterThan(x => x.kmInicial).WithMessage("O campo {PropertyName} deve ser maior que a quilometragem inicial.")
            .LessThan(x => decimal.MaxValue).WithMessage("O campo {PropertyName} deve ser menor que {ComparisonValue}.");

        When(x => x.tanqueCheio == false, () =>
        {
            RuleFor(x => x.porcentagemTanque)
                .NotNull().WithMessage("O campo {PropertyName} deve ser obrigatório.")
                .GreaterThanOrEqualTo(0).WithMessage("O campo {PropertyName} deve ser maior ou igual a zero.")
                .LessThan(1)
                .WithMessage("O campo {PropertyName} deve ser menor do que um.");
        });
    }
}
