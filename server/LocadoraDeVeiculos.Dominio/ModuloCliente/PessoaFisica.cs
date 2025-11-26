namespace LocadoraDeVeiculos.Dominio.ModuloCliente;

public class PessoaFisica : Cliente
{
    public string Cpf { get; set; }
    public string Rg { get; set; }
    public string Cnh { get; set; }
    public Guid? PessoaJuridicaId { get; set; }
    public PessoaJuridica? PessoaJuridica { get; set; }

    public PessoaFisica() { }

    public PessoaFisica(
        string nome,
        string telefone,
        string endereco,
        string cpf,
        string rg,
        string cnh,
        PessoaJuridica? pessoaJuridica = null
        ) : this()
    {
        Nome = nome;
        Telefone = telefone;
        Endereco = endereco;
        Cpf = cpf;
        Rg = rg;
        Cnh = cnh;
        PessoaJuridica = pessoaJuridica;
    }
}
