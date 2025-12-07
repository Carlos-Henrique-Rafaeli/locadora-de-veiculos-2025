using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;

namespace LocadoraDeVeiculos.Dominio.ModuloVeiculos;

public class Veiculo : EntidadeBase<Veiculo>
{
    public GrupoVeiculo GrupoVeiculo { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Marca { get; set; }
    public string Cor { get; set; }
    public TipoCombustivel TipoCombustivel { get; set; }
    public decimal CapacidadeTanque { get; set; }
    public byte[]? Imagem { get; set; }

    public Veiculo() { }

    public Veiculo(
        GrupoVeiculo grupoVeiculo, 
        string placa,
        string modelo, 
        string marca, 
        string cor, 
        TipoCombustivel tipoCombustivel, 
        decimal capacidadeTanque,
        byte[]? foto = null
        )
    {
        GrupoVeiculo = grupoVeiculo;
        Placa = placa;
        Modelo = modelo;
        Marca = marca;
        Cor = cor;
        TipoCombustivel = tipoCombustivel;
        CapacidadeTanque = capacidadeTanque;
        Imagem = foto;
    }

    public override void AtualizarRegistro(Veiculo registroEditado)
    {
        Placa = registroEditado.Placa;
        Modelo = registroEditado.Modelo;
        Marca = registroEditado.Marca;
        Cor = registroEditado.Cor;
        TipoCombustivel = registroEditado.TipoCombustivel;
        CapacidadeTanque = registroEditado.CapacidadeTanque;
        GrupoVeiculo = registroEditado.GrupoVeiculo;
        Imagem = registroEditado.Imagem;
    }
}
