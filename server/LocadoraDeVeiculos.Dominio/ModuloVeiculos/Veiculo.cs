using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;

namespace LocadoraDeVeiculos.Dominio.ModuloVeiculos;

public class Veiculo : EntidadeBase
{
    public GrupoVeiculo GrupoVeiculo { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Marca { get; set; }
    public string Cor { get; set; }
    public TipoCombustivel TipoCombustivel { get; set; }
    public decimal CapacidadeTanque { get; set; }

    public Veiculo() { }

    public Veiculo(
        GrupoVeiculo grupoVeiculo, 
        string placa,
        string modelo, 
        string marca, 
        string cor, 
        TipoCombustivel tipoCombustivel, 
        decimal capacidadeTanque)
    {
        GrupoVeiculo = grupoVeiculo;
        Placa = placa;
        Modelo = modelo;
        Marca = marca;
        Cor = cor;
        TipoCombustivel = tipoCombustivel;
        CapacidadeTanque = capacidadeTanque;
    }
}
