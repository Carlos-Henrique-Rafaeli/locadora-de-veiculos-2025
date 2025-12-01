using LocadoraDeVeiculos.Dominio.Compartilhado;

namespace LocadoraDeVeiculos.Dominio.ModuloConfiguracao;

public class ConfiguracaoPreco : EntidadeBase
{
    public decimal Gasolina { get; set; }
    public decimal Etanol { get; set; }
    public decimal Diesel { get; set; }

    public ConfiguracaoPreco() { }

    public ConfiguracaoPreco(
        decimal gasolina, 
        decimal etanol, 
        decimal diesel 
        ) : this()
    {
        Gasolina = gasolina;
        Etanol = etanol;
        Diesel = diesel;
    }
}
