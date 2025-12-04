using Microsoft.EntityFrameworkCore;
using LocadoraDeVeiculos.Dominio.Compartilhado;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;

public class RepositorioBase<TEntidade>(LocadoraDeVeiculosDbContext context) where TEntidade : EntidadeBase<TEntidade>
{
    protected readonly DbSet<TEntidade> registros = ((DbContext)context).Set<TEntidade>();

    public async Task InserirAsync(TEntidade registro)
    {
        await registros.AddAsync(registro);
    }

    public async Task<bool> EditarAsync(Guid idRegistro, TEntidade registro)
    {
        var registroSelecionado = await SelecionarPorIdAsync(idRegistro);

        if (registroSelecionado == null)
            return false;

        registroSelecionado.AtualizarRegistro(registro);

        return true;
    }

    public async Task<bool> ExcluirAsync(Guid idRegistro)
    {
        var registroSelecionado = await SelecionarPorIdAsync(idRegistro);
        
        if (registroSelecionado == null)
            return false;
        
        registroSelecionado.Excluir();
        
        return true;
    }

    public virtual async Task<List<TEntidade>> SelecionarTodosAsync()
    {
        return await registros.ToListAsync();
    }

    public virtual async Task<TEntidade?> SelecionarPorIdAsync(Guid id)
    {
        return await registros.SingleOrDefaultAsync(x => x.Id == id);
    }
}