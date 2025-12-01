using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloTaxaServico;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxaServico;

public class RepositorioTaxaServicoEmOrm(IContextoPersistencia context)
    : RepositorioBase<TaxaServico>(context), IRepositorioTaxaServico;