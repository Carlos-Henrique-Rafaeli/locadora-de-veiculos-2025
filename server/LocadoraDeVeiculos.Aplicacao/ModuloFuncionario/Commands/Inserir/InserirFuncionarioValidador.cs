using FluentValidation;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands.Inserir;

public class InserirFuncionarioValidador : AbstractValidator<InserirFuncionarioRequest>
{
    public InserirFuncionarioValidador()
    {
        RuleFor(p => p.NomeCompleto)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .MinimumLength(3).WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres.")
            .MaximumLength(100).WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres.");

        RuleFor(p => p.Cpf)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Matches(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$").WithMessage("O campo {PropertyName} deve estar no formato XXX.XXX.XXX-XX.");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .EmailAddress().WithMessage("O campo {PropertyName} deve conter um endereço de e-mail válido.")
            .MaximumLength(100).WithMessage("O campo {PropertyName} deve conter no máximo {MaxLength} caracteres."); 

        RuleFor(p => p.Senha)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .MinimumLength(6).WithMessage("A senha deve conter pelo menos {MinLength} caracteres.") // options.Password.RequiredLength = 6
            .Matches(@"[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula.")   // options.Password.RequireUppercase = true
            .Matches(@"[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula.")   // options.Password.RequireLowercase = true
            .Matches(@"[0-9]").WithMessage("A senha deve conter pelo menos um número.");            // options.Password.RequireDigit = true

        RuleFor(p => p.ConfirmarSenha)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .Equal(p => p.Senha).WithMessage("As senhas não conferem.");

        RuleFor(p => p.Salario)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
            .GreaterThan(0).WithMessage("O campo {PropertyName} precisa conter um valor maior que 0.");
    }
}

