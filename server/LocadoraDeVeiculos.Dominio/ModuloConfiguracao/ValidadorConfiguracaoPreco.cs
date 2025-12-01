using FluentValidation;

namespace LocadoraDeVeiculos.Dominio.ModuloConfiguracao;

public class ValidadorConfiguracaoPreco : AbstractValidator<ConfiguracaoPreco>
{
    public ValidadorConfiguracaoPreco()
    {
        RuleFor(x => x.Gasolina)
            .GreaterThan(0)
            .WithMessage("O campo {PropertyName} deve ser maior que zero.");

        RuleFor(x => x.Diesel)
            .GreaterThan(0)
            .WithMessage("O campo {PropertyName} deve ser maior que zero.");

        RuleFor(x => x.Etanol)
            .GreaterThan(0)
            .WithMessage("O campo {PropertyName} deve ser maior que zero.");
    }
}