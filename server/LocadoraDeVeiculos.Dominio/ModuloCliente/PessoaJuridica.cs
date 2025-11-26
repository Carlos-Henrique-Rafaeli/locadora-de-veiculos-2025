using LocadoraDeVeiculos.Dominio.ModuloCondutor;

namespace LocadoraDeVeiculos.Dominio.ModuloCliente;

public class PessoaJuridica : Cliente
{
    public string Cnpj { get; set; }
    public Condutor Condutor { get; set; }

    public PessoaJuridica() { }

    public PessoaJuridica(
        string nome,
        string telefone,
        string endereco,
        string cnpj,
        Condutor condutor
        ) : this()
    {
        Nome = nome;
        Telefone = telefone;
        Endereco = endereco;
        Cnpj = cnpj;
        Condutor = condutor;
    }
}
