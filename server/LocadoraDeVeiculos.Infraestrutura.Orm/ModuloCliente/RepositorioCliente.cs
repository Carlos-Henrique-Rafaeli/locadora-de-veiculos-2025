using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;

public class RepositorioPessoaJuridicaEmOrm(IContextoPersistencia context) 
    : RepositorioBase<PessoaJuridica>(context), IRepositorioPessoaJuridica
{
    public override async Task<List<PessoaJuridica>> SelecionarTodosAsync()
    {
        return await registros
            .Include(x => x.Condutor)
            .ToListAsync();
    }

    public override async Task<PessoaJuridica?> SelecionarPorIdAsync(Guid id)
    {
        return await registros
            .Include(x => x.Condutor)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}

public class RepositorioPessoaFisicaEmOrm(IContextoPersistencia context) 
    : RepositorioBase<PessoaFisica>(context), IRepositorioPessoaFisica
{
    public override async Task<List<PessoaFisica>> SelecionarTodosAsync()
    {
        return await registros
            .Include(x => x.PessoaJuridica)
            .ThenInclude(j => j.Condutor)
            .ToListAsync();
    }
    public override async Task<PessoaFisica?> SelecionarPorIdAsync(Guid id)
    {
        return await registros
            .Include(x => x.PessoaJuridica)
            .ThenInclude(j => j.Condutor)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
