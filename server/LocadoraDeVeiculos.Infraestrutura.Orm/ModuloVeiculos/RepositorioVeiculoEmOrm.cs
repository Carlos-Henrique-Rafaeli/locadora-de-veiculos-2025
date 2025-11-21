using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloVeiculos;

public class RepositorioVeiculoEmOrm(IContextoPersistencia context)
    : RepositorioBase<Veiculo>(context), IRepositorioVeiculo
{
    public override async Task<List<Veiculo>> SelecionarTodosAsync()
    {
        return await registros
            .Include(x => x.GrupoVeiculo)
            .ToListAsync();
    }

    public override async Task<Veiculo?> SelecionarPorIdAsync(Guid id)
    {
        return await registros
            .Include(x => x.GrupoVeiculo)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}