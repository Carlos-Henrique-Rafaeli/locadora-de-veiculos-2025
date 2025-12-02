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
    public List<TaxaServico> TaxasServicos { get; set; } = [];
    public bool EstaAberto { get; set; } = true;
    
    public readonly decimal ValorEntrada = 1000;

    public Aluguel() { }

    public Aluguel(
        Condutor condutor,
        GrupoVeiculo grupoVeiculo,
        Veiculo veiculo,
        DateTime dataEntrada,
        DateTime dataRetorno,
        PlanoCobranca planoCobranca,
        List<TaxaServico> taxasServicos) : this()
    {
        Condutor = condutor;
        GrupoVeiculo = grupoVeiculo;
        Veiculo = veiculo;
        DataEntrada = dataEntrada;
        DataRetorno = dataRetorno;
        PlanoCobranca = planoCobranca;
        TaxasServicos = taxasServicos;
    }

    public decimal CalcularValorTotal(decimal kmRodados = 0, bool atraso = false)
    {
        int diasDeUso = (DataRetorno - DataEntrada).Days;
        decimal valorPlano = 0;
        
        switch (PlanoCobranca.TipoPlano)
        {
            case TipoPlano.PlanoDiario:
                valorPlano = PlanoCobranca.ValorDiario!.Value * diasDeUso;
                break;
            
            case TipoPlano.PlanoControlado:
                var kmRestante = kmRodados - PlanoCobranca.KmIncluso!.Value;

                valorPlano = PlanoCobranca.ValorDiario!.Value * diasDeUso;

                if (kmRestante > 0) 
                    valorPlano += kmRestante * PlanoCobranca.ValorKmExcedente!.Value;

                break;
            
            case TipoPlano.PlanoLivre:
                valorPlano = PlanoCobranca.ValorFixo!.Value;
                break;
        }

        decimal valorTaxasServicos = 0;

        TaxasServicos.ForEach(x =>
        {
            switch (x.TipoCobranca)
            {
                case TipoCobranca.CobrancaDiaria:
                    valorTaxasServicos += x.Valor * diasDeUso;
                    
                    break;
                
                case TipoCobranca.PrecoFixo:
                    valorTaxasServicos += x.Valor;

                    break;

                default:
                    break;
            }
        });

        if (atraso)
            valorPlano *= 1.1m;

        return ValorEntrada + valorPlano + valorTaxasServicos;
    }
}
