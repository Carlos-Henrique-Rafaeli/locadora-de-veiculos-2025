using LocadoraDeVeiculos.Dominio.Compartilhado;

namespace LocadoraDeVeiculos.Dominio.ModuloCliente;

public class Cliente : EntidadeBase
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
}
