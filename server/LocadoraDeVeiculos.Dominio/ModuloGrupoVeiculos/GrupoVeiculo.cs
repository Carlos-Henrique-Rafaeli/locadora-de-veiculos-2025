using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;

namespace LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;

public class GrupoVeiculo : EntidadeBase<GrupoVeiculo>
{
    public string Nome { get; set; }
    public List<Veiculo> Veiculos { get; set; }

    public GrupoVeiculo()
    {
        Veiculos = [];
    }

    public GrupoVeiculo(string nome) : this()
    {
        Nome = nome;
    }

    public override void AtualizarRegistro(GrupoVeiculo registroEditado)
    {
        Nome = registroEditado.Nome;
    }
}
