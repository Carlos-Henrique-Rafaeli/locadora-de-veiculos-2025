using LocadoraDeVeiculos.Dominio.Compartilhado;

namespace LocadoraDeVeiculos.Dominio.ModuloTaxaServico;

public class TaxaServico : EntidadeBase<TaxaServico>
{
    public string Nome { get; set; }
    public decimal Valor { get; set; }
    public TipoCobranca TipoCobranca { get; set; }

    public TaxaServico() { }

    public TaxaServico(string nome, decimal valor, TipoCobranca tipoCobranca) : this()
    {
        Nome = nome;
        Valor = valor;
        TipoCobranca = tipoCobranca;
    }

    public override void AtualizarRegistro(TaxaServico registroEditado)
    {
        Nome = registroEditado.Nome;
        Valor = registroEditado.Valor;
        TipoCobranca = registroEditado.TipoCobranca;
    }
}
