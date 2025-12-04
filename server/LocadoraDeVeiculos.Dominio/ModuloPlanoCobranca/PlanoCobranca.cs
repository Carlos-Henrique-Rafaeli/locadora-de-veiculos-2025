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
        ValorDiario = valorDiario;
        ValorKm = valorKm;
        KmIncluso = kmIncluso;
        ValorKmExcedente = valorKmExcedente;
        ValorFixo = valorFixo;
    }

    public override void AtualizarRegistro(PlanoCobranca registroEditado)
    {
        TipoPlano = registroEditado.TipoPlano;
        GrupoVeiculo = registroEditado.GrupoVeiculo;
        ValorDiario = registroEditado.ValorDiario;
        ValorKm = registroEditado.ValorKm;
        KmIncluso = registroEditado.KmIncluso;
        ValorKmExcedente = registroEditado.ValorKmExcedente;
        ValorFixo = registroEditado.ValorFixo;
    }
}
