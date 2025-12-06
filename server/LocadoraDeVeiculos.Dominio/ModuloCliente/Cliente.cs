using LocadoraDeVeiculos.Dominio.Compartilhado;

namespace LocadoraDeVeiculos.Dominio.ModuloCliente;

public class Cliente : EntidadeBase<Cliente>
{
    public TipoCliente TipoCliente { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string? Cpf { get; set; }
    public string? Cnpj { get; set; }
    public TipoEstado Estado { get; set; }
    public string Cidade { get; set; }
    public string Bairro { get; set; }
    public string Rua { get; set; }
    public int Numero { get; set; }

    public Cliente() { }

    public Cliente(
        TipoCliente tipoCliente, 
        string nome, 
        string telefone, 
        string? cpf, 
        string? cnpj,
        TipoEstado estado, 
        string cidade, 
        string bairro, 
        string rua, 
        int numero) : this()
    {
        TipoCliente = tipoCliente;
        Nome = nome;
        Telefone = telefone;

        switch (TipoCliente)
        {
            case TipoCliente.PessoaFisica:
                Cpf = cpf;
                break;
            
            case TipoCliente.PessoaJuridica:
                Cnpj = cnpj;
                break;
            
            default:
                Cpf = cpf;
                Cnpj = cnpj;
                break;
        }

        Estado = estado;
        Cidade = cidade;
        Bairro = bairro;
        Rua = rua;
        Numero = numero;
    }

    public override void AtualizarRegistro(Cliente registroEditado)
    {
        TipoCliente = registroEditado.TipoCliente;
        Nome = registroEditado.Nome;
        Telefone = registroEditado.Telefone;

        switch (TipoCliente)
        {
            case TipoCliente.PessoaFisica:
                Cpf = registroEditado.Cpf;
                Cnpj = null;
                break;
            
            case TipoCliente.PessoaJuridica:
                Cnpj= registroEditado.Cnpj;
                Cpf = null;
                break;
            
            default:
                Cpf = registroEditado.Cpf;
                Cnpj = registroEditado.Cnpj;
                break;
        }
        
        Estado = registroEditado.Estado;
        Cidade = registroEditado.Cidade;
        Bairro = registroEditado.Bairro;
        Rua = registroEditado.Rua;
        Numero = registroEditado.Numero;
    }
}
