using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;

public class RepositorioClienteEmOrm(IContextoPersistencia context) 
    : RepositorioBase<Cliente>(context), IRepositorioCliente;