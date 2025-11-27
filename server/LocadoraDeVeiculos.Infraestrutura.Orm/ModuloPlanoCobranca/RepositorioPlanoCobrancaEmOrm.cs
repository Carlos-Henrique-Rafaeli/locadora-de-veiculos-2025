using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;

public class RepositorioPlanoCobrancaEmOrm(IContextoPersistencia context)
    : RepositorioBase<PlanoCobranca>(context), IRepositorioPlanoCobranca
{
    public override Task<List<PlanoCobranca>> SelecionarTodosAsync()
    {
        return registros
            .Include(x => x.GrupoVeiculo)
            .ToListAsync();
    }

    public override Task<PlanoCobranca?> SelecionarPorIdAsync(Guid id)
    {
        return registros
            .Include(x => x.GrupoVeiculo)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
