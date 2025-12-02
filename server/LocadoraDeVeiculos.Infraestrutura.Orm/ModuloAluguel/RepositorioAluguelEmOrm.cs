using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;

public class RepositorioAluguelEmOrm(IContextoPersistencia context)
    : RepositorioBase<Aluguel>(context), IRepositorioAluguel
{
    public override async Task<List<Aluguel>> SelecionarTodosAsync()
    {
        return await registros
            .Include(x => x.Condutor)
            .Include(x => x.GrupoVeiculo)
            .Include(x => x.Veiculo)
            .Include(x => x.PlanoCobranca)
            .Include(x => x.TaxasServicos)
            .ToListAsync();
    }

    public override async Task<Aluguel?> SelecionarPorIdAsync(Guid id)
    {
        return await registros
            .Include(x => x.Condutor)
            .Include(x => x.GrupoVeiculo)
            .Include(x => x.Veiculo)
            .Include(x => x.PlanoCobranca)
            .Include(x => x.TaxasServicos)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}