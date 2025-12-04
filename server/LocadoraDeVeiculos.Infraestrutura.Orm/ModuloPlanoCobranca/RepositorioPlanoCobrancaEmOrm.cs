using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;

public class RepositorioPlanoCobrancaEmOrm(LocadoraDeVeiculosDbContext context)
    : RepositorioBase<PlanoCobranca>(context), IRepositorioPlanoCobranca
{
    public override async Task<List<PlanoCobranca>> SelecionarTodosAsync()
    {
        return await registros
            .Include(x => x.GrupoVeiculo)
            .ToListAsync();
    }

    public override async Task<PlanoCobranca?> SelecionarPorIdAsync(Guid id)
    {
        return await registros
            .Include(x => x.GrupoVeiculo)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
