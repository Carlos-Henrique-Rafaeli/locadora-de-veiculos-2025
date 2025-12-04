using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;

namespace LocadoraDeVeiculos.Dominio.ModuloCondutor;

public class Condutor : EntidadeBase<Condutor>
{
    public Cliente Cliente { get; set; }
    public bool ClienteCondutor { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public string Cnh { get; set; }
    public DateTime ValidadeCnh { get; set; }
    public string Telefone { get; set; }

    public Condutor() { }

    public Condutor(
        Cliente cliente,
        bool clienteCondutor,
        string nome,
        string email, 
        string cpf, 
        string cnh, 
        DateTime validadeCnh, 
        string telefone) : this()
    {
        Cliente = cliente;
        ClienteCondutor = clienteCondutor;
        Nome = nome;
        Email = email;
        Cpf = cpf;
        Cnh = cnh;
        ValidadeCnh = validadeCnh;
        Telefone = telefone;
    }

    public override void AtualizarRegistro(Condutor registroEditado)
    {
        Cliente = registroEditado.Cliente;
        ClienteCondutor = registroEditado.ClienteCondutor;
        Nome = registroEditado.Nome;
        Email = registroEditado.Email;
        Cpf = registroEditado.Cpf;
        Cnh = registroEditado.Cnh;
        ValidadeCnh = registroEditado.ValidadeCnh;
        Telefone = registroEditado.Telefone;
    }
}
