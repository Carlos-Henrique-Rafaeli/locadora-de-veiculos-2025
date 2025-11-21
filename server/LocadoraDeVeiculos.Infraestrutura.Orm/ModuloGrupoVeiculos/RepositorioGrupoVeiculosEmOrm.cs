using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloGrupoVeiculos;

public class RepositorioGrupoVeiculosEmOrm(IContextoPersistencia context)
    : RepositorioBase<GrupoVeiculo>(context), IRepositorioGrupoVeiculos
{
    public override async Task<List<GrupoVeiculo>> SelecionarTodosAsync()
    {
        return await registros
            .Include(x => x.Veiculos)
            .ToListAsync();
    }

    public override async Task<GrupoVeiculo?> SelecionarPorIdAsync(Guid id)
    {
        return await registros
            .Include(x => x.Veiculos)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
