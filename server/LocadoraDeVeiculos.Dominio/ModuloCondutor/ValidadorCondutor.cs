using FluentValidation;

namespace LocadoraDeVeiculos.Dominio.ModuloCondutor;

public class ValidadorCondutor : AbstractValidator<Condutor>
{
    public ValidadorCondutor()
    {
        RuleFor(x => x.Cliente)
            .NotNull().WithMessage("O campo {PropertyName} é obrigatório.");

        RuleFor(x => x.ClienteCondutor)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .MinimumLength(3).WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres.")
            .MaximumLength(100).WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .EmailAddress().WithMessage("O campo {PropertyName} deve ser um endereço de email válido.");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Matches(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$").WithMessage("O campo {PropertyName} deve estar no formato XXX.XXX.XXX-XX.");

        RuleFor(x => x.Cnh)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Matches(@"^\d{11}$").WithMessage("O campo {PropertyName} deve conter 11 dígitos numéricos.");

        RuleFor(x => x.ValidadeCnh)
            .NotNull().WithMessage("O campo {PropertyName} é obrigatório.")
            .Must(d => d > DateTime.Now).WithMessage("O campo {PropertyName} deve ser uma data futura.");

        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Matches(@"^\(\d{2}\) \d{5}-\d{4}$").WithMessage("O campo {PropertyName} deve estar no formato (XX) XXXXX-XXXX.");
    }
}
