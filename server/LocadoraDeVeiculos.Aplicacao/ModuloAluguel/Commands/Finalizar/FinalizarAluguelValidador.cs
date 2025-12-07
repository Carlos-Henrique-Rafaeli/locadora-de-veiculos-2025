using FluentValidation;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands.Finalizar;

public class FinalizarAluguelValidador : AbstractValidator<FinalizarAluguelRequest>
{
    public FinalizarAluguelValidador()
    {
        RuleFor(x => x.kmInicial)
            .GreaterThan(0)
            .WithMessage("O campo {PropertyName} deve ser maior que zero.")
            .LessThanOrEqualTo(x => x.kmAtual)
            .WithMessage("O campo {PropertyName} não pode ser maior que a quilometragem atual.");

        RuleFor(x => x.kmAtual)
            .GreaterThanOrEqualTo(x => x.kmInicial)
            .WithMessage("O campo {PropertyName} deve ser maior ou igual a quilometragem inicial.");

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
