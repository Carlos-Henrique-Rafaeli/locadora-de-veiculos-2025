using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;

namespace LocadoraDeVeiculos.Dominio.ModuloAluguel;

public class Aluguel : EntidadeBase
{
    public Condutor Condutor { get; set; }
    public GrupoVeiculo GrupoVeiculo { get; set; }
    public Veiculo Veiculo { get; set; }
    public DateTime DataEntrada { get; set; }
    public DateTime DataRetorno { get; set; }
    public PlanoCobranca PlanoCobranca { get; set; }
    public List<TaxaServico> TaxasServicos { get; set; }

    public readonly decimal ValorEntrada = 1000;

    public Aluguel() { }

    public Aluguel(
        Condutor condutor,
        GrupoVeiculo grupoVeiculo,
        Veiculo veiculo,
        DateTime dataEntrada,
        DateTime dataRetorno,
        PlanoCobranca planoCobranca,
        List<TaxaServico>? taxasServicos = null) : this()
    {
        Condutor = condutor;
        GrupoVeiculo = grupoVeiculo;
        Veiculo = veiculo;
        DataEntrada = dataEntrada;
        DataRetorno = dataRetorno;
        PlanoCobranca = planoCobranca;
        TaxasServicos = taxasServicos ?? [];
    }
}
