using LocadoraDeVeiculos.Dominio.Compartilhado;

namespace LocadoraDeVeiculos.Dominio.ModuloCondutor;

public class Condutor : EntidadeBase
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public string Cnh { get; set; }
    public DateTime ValidadeCnh { get; set; }
    public string Telefone { get; set; }

    public Condutor(
        string nome,
        string email, 
        string cpf, 
        string cnh, 
        DateTime validadeCnh, 
        string telefone)
    {
        Nome = nome;
        Email = email;
        Cpf = cpf;
        Cnh = cnh;
        ValidadeCnh = validadeCnh;
        Telefone = telefone;
    }
}
