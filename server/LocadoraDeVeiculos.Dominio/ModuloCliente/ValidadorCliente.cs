using FluentValidation;

namespace LocadoraDeVeiculos.Dominio.ModuloCliente;

public class ValidadorCliente : AbstractValidator<Cliente>
{
    public ValidadorCliente()
    {
        RuleFor(x => x.TipoCliente)
            .IsInEnum().WithMessage("O campo {PropertyName} possui um valor inválido.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .MinimumLength(3).WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres.")
            .MaximumLength(100).WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres.");

        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Matches(@"^\(\d{2}\) 9\d{4}-\d{4}$").WithMessage("O campo {PropertyName} deve seguir o formato (XX) XXXXX-XXXX.");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Matches(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$").WithMessage("O campo {PropertyName} deve estar no formato XXX.XXX.XXX-XX.");

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Matches(@"^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$").WithMessage("O campo {PropertyName} deve estar no formato XX.XXX.XXX/XXXX-XX.");

        RuleFor(x => x.Estado)
            .IsInEnum().WithMessage("O campo {PropertyName} possui um valor inválido.");

        RuleFor(x => x.Cidade)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .MinimumLength(3).WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres.")
            .MaximumLength(50).WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres.");

        RuleFor(x => x.Cidade)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .MinimumLength(3).WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres.")
            .MaximumLength(50).WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres.");

        RuleFor(x => x.Rua)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .MinimumLength(3).WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres.")
            .MaximumLength(100).WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres.");

        RuleFor(x => x.Numero)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.");
    }
}
