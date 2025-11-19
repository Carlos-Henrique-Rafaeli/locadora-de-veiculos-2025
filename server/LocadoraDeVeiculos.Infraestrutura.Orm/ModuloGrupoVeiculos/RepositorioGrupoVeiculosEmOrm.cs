using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloGrupoVeiculos;

public class RepositorioGrupoVeiculosEmOrm(IContextoPersistencia context)
    : RepositorioBase<GrupoVeiculo>(context), IRepositorioGrupoVeiculos;
