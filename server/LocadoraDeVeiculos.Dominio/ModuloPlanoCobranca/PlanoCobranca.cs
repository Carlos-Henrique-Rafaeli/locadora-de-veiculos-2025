using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;

namespace LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;

public class PlanoCobranca : EntidadeBase<PlanoCobranca>
{
    public TipoPlano TipoPlano { get; set; }
    public GrupoVeiculo GrupoVeiculo { get; set; }
    public decimal? ValorDiario { get; set; }
    public decimal? ValorKm { get; set; }
    public int? KmIncluso { get; set; }
    public decimal? ValorKmExcedente { get; set; }
    public decimal? ValorFixo { get; set; }

    public PlanoCobranca() { }

    public PlanoCobranca(
        TipoPlano tipoPlano,
        GrupoVeiculo grupoVeiculo,
        decimal? valorDiario = null,
        decimal? valorKm = null,
        int? kmIncluso = null,
        decimal? valorKmExcedente = null,
        decimal? valorFixo = null
        ) : this()
    {
        TipoPlano = tipoPlano;
        GrupoVeiculo = grupoVeiculo;

        switch (TipoPlano)
        {
            case TipoPlano.PlanoDiario:
                ValorDiario = valorDiario;
                ValorKm = valorKm;
                break;

            case TipoPlano.PlanoControlado:
                ValorDiario = valorDiario;
                KmIncluso = kmIncluso;
                ValorKmExcedente = valorKmExcedente;
                break;

            case TipoPlano.PlanoLivre:
                ValorFixo = valorFixo;
                break;

            default:
                ValorDiario = valorDiario;
                ValorKm = valorKm;
                KmIncluso = kmIncluso;
                ValorKmExcedente = valorKmExcedente;
                ValorFixo = valorFixo;
                break;
        }
    }

    public override void AtualizarRegistro(PlanoCobranca registroEditado)
    {
        TipoPlano = registroEditado.TipoPlano;
        GrupoVeiculo = registroEditado.GrupoVeiculo;

        switch (TipoPlano)
        {
            case TipoPlano.PlanoDiario:
                ValorDiario = registroEditado.ValorDiario;
                ValorKm = registroEditado.ValorKm;
                KmIncluso = null;
                ValorKmExcedente = null;
                ValorFixo = null;
                break;
            
            case TipoPlano.PlanoControlado:
                ValorDiario = registroEditado.ValorDiario;
                ValorKm = null;
                KmIncluso = registroEditado.KmIncluso;
                ValorKmExcedente = registroEditado.ValorKmExcedente;
                ValorFixo = null;
                break;
            
            case TipoPlano.PlanoLivre:
                ValorDiario = null;
                ValorKm = null;
                KmIncluso = null;
                ValorKmExcedente = null;
                ValorFixo = registroEditado.ValorFixo;
                break;
            
            default:
                ValorDiario = registroEditado.ValorDiario;
                ValorKm = registroEditado.ValorKm;
                KmIncluso = registroEditado.KmIncluso;
                ValorKmExcedente = registroEditado.ValorKmExcedente;
                ValorFixo = registroEditado.ValorFixo;
                break;
        }
    }
}
