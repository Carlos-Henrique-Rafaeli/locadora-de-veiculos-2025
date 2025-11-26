using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;

public class RepositorioPessoaJuridicaEmOrm(IContextoPersistencia context) 
    : RepositorioBase<PessoaJuridica>(context), IRepositorioPessoaJuridica
{
    public override Task<List<PessoaJuridica>> SelecionarTodosAsync()
    {
        return registros
            .Include(x => x.Condutor)
            .ToListAsync();
    }

    public override Task<PessoaJuridica?> SelecionarPorIdAsync(Guid id)
    {
        return registros
            .Include(x => x.Condutor)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}

public class RepositorioPessoaFisicaEmOrm(IContextoPersistencia context) 
    : RepositorioBase<PessoaFisica>(context), IRepositorioPessoaFisica
{
    public override Task<List<PessoaFisica>> SelecionarTodosAsync()
    {
        return registros
            .Include(x => x.PessoaJuridica)
            .ToListAsync();
    }
    public override Task<PessoaFisica?> SelecionarPorIdAsync(Guid id)
    {
        return registros
            .Include(x => x.PessoaJuridica)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
