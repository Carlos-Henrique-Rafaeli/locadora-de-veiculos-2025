using LocadoraDeVeiculos.Dominio.Compartilhado;

namespace LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;

public class GrupoVeiculo : EntidadeBase
{
    public string Nome { get; set; }
    
    public GrupoVeiculo(string nome) 
    {
        Nome = nome;
    }
}
