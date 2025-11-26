using FluentValidation;

namespace LocadoraDeVeiculos.Dominio.ModuloCliente;

public class ValidadorCliente : AbstractValidator<Cliente>
{
    public ValidadorCliente()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .MinimumLength(3).WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres.")
            .MaximumLength(100).WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres.");

        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Matches(@"^\(\d{2}\) 9\d{4}-\d{4}$").WithMessage("O campo {PropertyName} deve seguir o formato (XX) XXXXX-XXXX.");

        RuleFor(x => x.Endereco)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .MinimumLength(5).WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres.")
            .MaximumLength(200).WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres.");
    }
}

public class ValidadorPessoaJuridica : AbstractValidator<PessoaJuridica>
{
    public ValidadorPessoaJuridica()
    {
        Include(new ValidadorCliente());

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Matches(@"^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$").WithMessage("O campo {PropertyName} deve estar no formato XX.XXX.XXX/XXXX-XX.");

        RuleFor(x => x.Condutor)
            .NotNull().WithMessage("O campo {PropertyName} é obrigatório.");
    }
}

public class ValidadorPessoaFisica : AbstractValidator<PessoaFisica>
{
    public ValidadorPessoaFisica()
    {
        Include(new ValidadorCliente());

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Matches(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$").WithMessage("O campo {PropertyName} deve estar no formato XXX.XXX.XXX-XX.");
        
        RuleFor(x => x.Rg)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Matches(@"^(?:\d{3}\.\d{3}\.\d{3}-\d{2}|\d{2}\.\d{3}\.\d{3}-[\dX])").WithMessage("O campo {PropertyName} deve estar no formato RG XX.XXX.XXX-X ou CPF XXX.XXX.XXX-XX.");
        
        RuleFor(x => x.Cnh)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Matches(@"^\d{11}$").WithMessage("O campo {PropertyName} deve conter 11 dígitos numéricos.");
    }
}

