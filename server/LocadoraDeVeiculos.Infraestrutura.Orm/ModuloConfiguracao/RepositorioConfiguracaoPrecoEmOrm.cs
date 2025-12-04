using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloConfiguracao;

public class RepositorioConfiguracaoPrecoEmOrm(LocadoraDeVeiculosDbContext context)
    : RepositorioBase<ConfiguracaoPreco>(context), IRepositorioConfiguracaoPreco;
